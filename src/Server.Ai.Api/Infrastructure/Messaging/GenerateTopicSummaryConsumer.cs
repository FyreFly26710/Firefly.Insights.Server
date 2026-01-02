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
        var parentJob = await _aiContext.JobLogs.FindAsync(message.ParentJobId);
        if (parentJob == null) return; // do nothing
        
        var topicSummaryJob = new JobLog()
        {
            UserId = parentJob.UserId,
            JobType = AiJobType.TopicSummaryGeneration,
            AiModelId = parentJob.AiModelId,
            Status = AiGenerationJobStatus.Running,
            CreatedAt = DateTime.UtcNow,
            StartedAt = DateTime.UtcNow,
        };
        _aiContext.JobLogs.Add(topicSummaryJob);
        await _aiContext.SaveChangesAsync(context.CancellationToken);

        try
        {
            // get topic
            var topicSummaryRequestMessage = new GetTopicRequestMessage(message.TopicId, true);
            var topicSummaryResponse = await _messageBus.RequestAsync<GetTopicRequestMessage, GetTopicRequestMessageResponse>(topicSummaryRequestMessage, context.CancellationToken);

            // Generate topic summary
            var articleContent = await _articleGenerationClient.GenerateTopicSummaryAsync(topicSummaryJob.Id, topicSummaryJob.AiModelId, topicSummaryResponse.Topic, context.CancellationToken);
            // Create article
            var createArticleRequestMessage = new CreateArticleRequestMessage("Summary Article", message.TopicId, 0, articleContent);
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
