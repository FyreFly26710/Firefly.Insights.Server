using System;
using Server.Ai.Api.Infrastructure.StateMachines;
using Server.Messages.Contents;

namespace Server.Ai.Api.Infrastructure.Messaging;

public class GenerateArticleContentConsumer
    (ILogger<GenerateArticleContentConsumer> _logger,
    IArticleGenerationClient _articleGenerationClient,
    AiContext _aiContext,
    IMessageBus _messageBus)
    : IConsumer<GenerateArticleContentCommand>
{
    public async Task Consume(ConsumeContext<GenerateArticleContentCommand> context)
    {
        var message = context.Message;
        var article = message.ArticleSummary;
        var topicId = message.TopicId;
        var userId = message.UserId;
        var parentJobLogId = message.ParentJobLogId;
        var jobLogId = message.JobLogId;
        // TODO: Add job follow up
        // _aiContext.JobFollowUps.Add(new JobFollowUp { ParentJobLogId = parentJobLogId, JobLogId = jobLogId });
        var jobQuery = _aiContext.JobLogs.AsQueryable().Include(j => j.AiModel);
        var job = await jobQuery.FirstOrDefaultAsync(j => j.Id == jobLogId);
        if (job == null)
        {
            _logger.LogCritical("Stopping the Job. Job log not found. JobLogId: {JobLogId}", jobLogId);
            return;
        }

        job.Status = AiGenerationJobStatus.Running;
        job.StartedAt = DateTime.UtcNow;
        await _aiContext.SaveChangesAsync();

        // get topic
        var topicSummaryRequestMessage = new GetTopicRequestMessage(topicId, false);
        var topicSummaryResponse = await _messageBus.RequestAsync<GetTopicRequestMessage, GetTopicRequestMessageResponse>(topicSummaryRequestMessage, context.CancellationToken);
        var topic = topicSummaryResponse.Topic;
        if (topic.TopicId != topicId)
        {
            _logger.LogError("Stopping the Job. Topic mismatch. JobLogId: {JobLogId}, TopicId: {TopicId}, TopicName: {TopicName}, Returned TopicId: {ReturnedTopicId}", jobLogId, topicId, topic.Name, topic.TopicId);
            await JobFailed(message.CorrelationId, job, "Topic mismatch", context);
            return;
        }

        try
        {
            // Generate article content
            var articleContent = await _articleGenerationClient.GenerateArticleContentAsync(job.Id, job.AiModelId, article, topic, context.CancellationToken);

            // get agent author info
            var getAgentRequestMessage = new UserRequestMessage(job.AiModelId);
            var getAgentResponse = await _messageBus.RequestAsync<UserRequestMessage, UserRequestMessageResponse>(getAgentRequestMessage, context.CancellationToken);
            var agent = getAgentResponse.UserTo;

            // Create article
            var createArticleRequestMessage = new CreateArticleRequestMessage(article.Title, topicId, agent.UserId, articleContent, article.Description, article.SortNumber, article.SkillLevelTag, article.FocusAreaTag, article.ArticleStyleTag, article.TechStackTag, article.ToneTag);
            var createArticleResponse = await _messageBus.RequestAsync<CreateArticleRequestMessage, CreateArticleRequestMessageResponse>(createArticleRequestMessage, context.CancellationToken);
            if (createArticleResponse.ArticleId <= 0)
                throw new Exception("Creating the article failed");

            var articleJob = new ArticleJob { JobLogId = job.Id, ArticleId = createArticleResponse.ArticleId };
            _aiContext.ArticleJobs.Add(articleJob);

            job.Status = AiGenerationJobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await _aiContext.SaveChangesAsync();

            // Publish the event
            await context.Publish(new ArticleContentGenerated(message.CorrelationId, job.Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating article content. JobLogId: {JobLogId}, ArticleTitle: {ArticleTitle}", jobLogId, article.Title);
            await JobFailed(message.CorrelationId, job, ex.Message, context);
        }
    }

    private async Task JobFailed(Guid correlationId, JobLog job, string failureReason, ConsumeContext<GenerateArticleContentCommand> context)
    {
        // Publish the event
        await context.Publish(new ArticleContentGenerationFailed(correlationId, job.Id, failureReason));

        job.Status = AiGenerationJobStatus.Failed;
        job.CompletedAt = DateTime.UtcNow;
        job.FailureReason = failureReason;
        await _aiContext.SaveChangesAsync();
    }
}
