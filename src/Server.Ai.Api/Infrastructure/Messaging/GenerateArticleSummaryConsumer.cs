using System;
using System.Text.Json;
using Server.Ai.Api.Infrastructure.StateMachines;
using Server.Messages.Ais;
using Server.Messages.Contents;

namespace Server.Ai.Api.Infrastructure.Messaging;

// Consume the GenerateArticleSummaryMessage
// Update the job log status to Running
// Generate the article summary list
// Update the job log status to Completed
public class GenerateArticleSummaryConsumer(
    ILogger<GenerateArticleSummaryConsumer> _logger,
    IArticleGenerationClient _articleGenerationClient,
    AiContext _aiContext,
    IMessageBus _messageBus)
    : IConsumer<GenerateArticleSummaryMessage>
{
    public async Task Consume(ConsumeContext<GenerateArticleSummaryMessage> context)
    {
        var message = context.Message;

        var job = await _aiContext.JobLogs.FindAsync(message.JobId);
        job!.Status = AiGenerationJobStatus.Running;
        job.StartedAt = DateTime.UtcNow;
        await _aiContext.SaveChangesAsync(context.CancellationToken);
        try
        {
            // generate the article summary list
            var responseMessage = await _articleGenerationClient.GenerateArticleSummaryListAsync(message.JobId, job.AiModelId, message.ArticleCount, message.Topic, message.TopicDescription, message.Category, message.Prompt, context.CancellationToken);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var articleList = JsonSerializer.Deserialize<GenerationArticleList>(responseMessage, options);

            if (articleList == null)
            {
                job.Status = AiGenerationJobStatus.Failed;
                job.CompletedAt = DateTime.UtcNow;
                job.FailureReason = "Invalid agent response message";
                await _aiContext.SaveChangesAsync(context.CancellationToken);
                return;
            }
            else
            {
                // add topic id to article list
                var createTopicMessage = new CreateTopicRequestMessage(message.CategoryId, message.Topic, message.TopicDescription, message.TopicUrl);
                var createTopicResponse = await _messageBus.RequestAsync<CreateTopicRequestMessage, CreateTopicRequestMessageResponse>(createTopicMessage, context.CancellationToken);

                var sagaArticles = new List<ArticleJobItem>();
                foreach (var summary in articleList.Articles)
                {
                    var articleJob = new JobLog
                    {
                        UserId = message.UserId,
                        JobType = AiJobType.Article_Generation,
                        AiModelId = job.AiModelId,
                        Status = AiGenerationJobStatus.Pending
                    };
                    _aiContext.JobLogs.Add(articleJob);

                    sagaArticles.Add(new ArticleJobItem(articleJob.Id, summary));
                }

                job.Status = AiGenerationJobStatus.Completed;
                job.CompletedAt = DateTime.UtcNow;
                await _aiContext.SaveChangesAsync(context.CancellationToken);

                // Publish the saga
                await context.Publish(new StartArticleBatchGeneration
                {
                    CorrelationId = Guid.NewGuid(),
                    ParentJobId = message.JobId,
                    TopicId = createTopicResponse.TopicId,
                    UserId = message.UserId,
                    AiModelId = job.AiModelId,
                    Articles = sagaArticles
                });
            }

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