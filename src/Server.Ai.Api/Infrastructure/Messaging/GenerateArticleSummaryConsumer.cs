using System;
using Server.Messages.Ais;

namespace Server.Ai.Api.Infrastructure.Messaging;

// Consume the GenerateArticleSummaryMessage
// Update the job log status to Running
// Generate the article summary list
// Update the job log status to Completed
public class GenerateArticleSummaryConsumer(
    ILogger<GenerateArticleSummaryConsumer> _logger,
    IArticleGenerationClient _articleGenerationClient,
    AiContext _aiContext)
    : IConsumer<GenerateArticleSummaryMessage>
{
    public async Task Consume(ConsumeContext<GenerateArticleSummaryMessage> context)
    {
        var message = context.Message;

        var job = await _aiContext.JobLogs.FindAsync(message.JobId);
        if (job == null)
        {
            _logger.LogError("Job not found: {JobId}", message.JobId);
            throw new ExceptionNotFound($"Job not found: {message.JobId}");
        }
        job.Status = AiGenerationJobStatus.Running;
        job.StartedAt = DateTime.UtcNow;
        await _aiContext.SaveChangesAsync(context.CancellationToken);
        try
        {
            await _articleGenerationClient.GenerateArticleSummaryListAsync(message, context.CancellationToken);
            job.Status = AiGenerationJobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await _aiContext.SaveChangesAsync(context.CancellationToken);

            // todo: publish a follow up message to generate the articles
        }
        catch (Exception ex)
        {
            job.Status = AiGenerationJobStatus.Failed;
            job.CompletedAt = DateTime.UtcNow;
            job.FailureReason = ex.Message;
            await _aiContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}