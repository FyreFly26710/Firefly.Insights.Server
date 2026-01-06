using System;
using Server.Ai.Api.Infrastructure.StateMachines;
using Server.Messages.Contents;

namespace Server.Ai.Api.Infrastructure.Messaging;

public class GenerateTopicSummaryConsumer(ILogger<GenerateTopicSummaryConsumer> _logger,
    IArticleGenerationClient _articleGenerationClient,
    AiContext _aiContext,
    IMessageBus _messageBus)
    : IConsumer<GenerateTopicSummaryCommand>
{
    public async Task Consume(ConsumeContext<GenerateTopicSummaryCommand> context)
    {
        var message = context.Message;
        var parentJobQuery = _aiContext.JobLogs.AsQueryable().AsNoTracking();
        parentJobQuery = parentJobQuery.Include(j => j.AiModel);
        var parentJob = await parentJobQuery.FirstOrDefaultAsync(j => j.Id == message.ParentJobId);
        if (parentJob == null) return; // do nothing

        var topicSummaryJob = new JobLog()
        {
            UserId = parentJob.UserId,
            JobType = AiJobType.Topic_Article_Generation,
            AiModelId = parentJob.AiModelId,
            Status = AiGenerationJobStatus.Running,
            CreatedAt = DateTime.UtcNow,
            StartedAt = DateTime.UtcNow,
        };
        _aiContext.JobLogs.Add(topicSummaryJob);
        await _aiContext.SaveChangesAsync(context.CancellationToken);

        try
        {
            // get agent
            var getAgentRequestMessage = new UserRequestMessage(parentJob.AiModelId);
            var getAgentResponse = await _messageBus.RequestAsync<UserRequestMessage, UserRequestMessageResponse>(getAgentRequestMessage, context.CancellationToken);
            var agent = getAgentResponse.UserTo;

            // get topic
            var topicSummaryRequestMessage = new GetTopicRequestMessage(message.TopicId, true);
            var topicSummaryResponse = await _messageBus.RequestAsync<GetTopicRequestMessage, GetTopicRequestMessageResponse>(topicSummaryRequestMessage, context.CancellationToken);

            // Generate topic summary
            var articleContent = await _articleGenerationClient.GenerateTopicSummaryAsync(topicSummaryJob.Id, topicSummaryJob.AiModelId, topicSummaryResponse.Topic, context.CancellationToken);
            // Create article
            var createArticleRequestMessage = new CreateArticleRequestMessage("Summary Article", message.TopicId, agent.UserId, articleContent, IsTopicSummary: true);
            await _messageBus.RequestAsync<CreateArticleRequestMessage, CreateArticleRequestMessageResponse>(createArticleRequestMessage, context.CancellationToken);
            topicSummaryJob.Status = AiGenerationJobStatus.Completed;
            topicSummaryJob.CompletedAt = DateTime.UtcNow;
            await _aiContext.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating topic summary");
            topicSummaryJob.Status = AiGenerationJobStatus.Failed;
            topicSummaryJob.CompletedAt = DateTime.UtcNow;
            topicSummaryJob.FailureReason = ex.Message;
            await _aiContext.SaveChangesAsync(context.CancellationToken);
        }
    }

}
