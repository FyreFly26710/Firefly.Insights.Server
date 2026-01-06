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
        var jobQuery = _aiContext.JobLogs.AsQueryable();
        jobQuery = jobQuery.Include(j => j.AiModel);
        var job = await jobQuery.FirstOrDefaultAsync(j => j.Id == jobLogId);
        job!.Status = AiGenerationJobStatus.Running;
        job.StartedAt = DateTime.UtcNow;
        await _aiContext.SaveChangesAsync(context.CancellationToken);

        // get topic
        var topicSummaryRequestMessage = new GetTopicRequestMessage(topicId, false);
        var topicSummaryResponse = await _messageBus.RequestAsync<GetTopicRequestMessage, GetTopicRequestMessageResponse>(topicSummaryRequestMessage, context.CancellationToken);
        var topic = topicSummaryResponse.Topic;

        // get agent
        var getAgentRequestMessage = new UserRequestMessage(job.AiModelId);
        var getAgentResponse = await _messageBus.RequestAsync<UserRequestMessage, UserRequestMessageResponse>(getAgentRequestMessage, context.CancellationToken);
        var agent = getAgentResponse.UserTo;
        try
        {
            // Generate article content
            var articleContent = await _articleGenerationClient.GenerateArticleContentAsync(job.Id, job.AiModelId, article, topic, context.CancellationToken);
            // Create article
            var createArticleRequestMessage = new CreateArticleRequestMessage(article.Title, topicId, agent.UserId, articleContent, article.Description, article.SortNumber, article.SkillLevelTag, article.FocusAreaTag, article.ArticleStyleTag, article.TechStackTag, article.ToneTag);
            var createArticleResponse = await _messageBus.RequestAsync<CreateArticleRequestMessage, CreateArticleRequestMessageResponse>(createArticleRequestMessage, context.CancellationToken);

            // Publish the event
            await context.Publish(new ArticleContentGenerated(message.CorrelationId, job.Id));

            var articleJob = new ArticleJob { JobLogId = job.Id, ArticleId = createArticleResponse.ArticleId };
            _aiContext.ArticleJobs.Add(articleJob);

            job.Status = AiGenerationJobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await _aiContext.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating article content: {ArticleTitle}", article.Title);
            // Publish the event
            await context.Publish(new ArticleContentGenerationFailed(message.CorrelationId, job.Id, ex.Message));

            job.Status = AiGenerationJobStatus.Failed;
            job.CompletedAt = DateTime.UtcNow;
            job.FailureReason = ex.Message;
            await _aiContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
