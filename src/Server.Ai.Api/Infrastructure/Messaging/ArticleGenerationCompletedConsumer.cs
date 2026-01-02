// using System;

// namespace Server.Ai.Api.Infrastructure.Messaging;

// public class ArticleGenerationCompletedConsumer
//     (ILogger<ArticleGenerationCompletedConsumer> _logger,
//     AiContext _aiContext,
//     IMessageBus _messageBus)
//     : IConsumer<ArticleGenerationCompletedMessage>
// {
//     public async Task Consume(ConsumeContext<ArticleGenerationCompletedMessage> context)
//     {
//         var message = context.Message;
//         var job = await _aiContext.JobLogs.FindAsync(message.JobLogId);
//         job.Status = AiGenerationJobStatus.Completed;
//         job.CompletedAt = DateTime.UtcNow;
//         await _aiContext.SaveChangesAsync(context.CancellationToken);
//     }

// }

// public record ArticleGenerationCompletedMessage(long JobLogId);