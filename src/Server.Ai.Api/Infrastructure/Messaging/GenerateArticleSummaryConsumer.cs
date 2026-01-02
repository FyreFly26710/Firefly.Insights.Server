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
        var jobId = message.JobId;
        var userId = message.UserId;
        // var aiModelId = message.AiModelId;
        var articleCount = message.ArticleCount;
        var topic = message.Topic;
        var topicDescription = message.TopicDescription;
        var category = message.Category;
        var userPrompt = message.Prompt;
        var categoryId = message.CategoryId;

        var job = await _aiContext.JobLogs.FindAsync(jobId);
        job!.Status = AiGenerationJobStatus.Running;
        job.StartedAt = DateTime.UtcNow;
        await _aiContext.SaveChangesAsync(context.CancellationToken);
        try
        {
            // generate the article summary list
            var responseMessage = await _articleGenerationClient.GenerateArticleSummaryListAsync(jobId, job.AiModelId, articleCount, topic, topicDescription, category, userPrompt, context.CancellationToken);
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
                var createTopicMessage = new CreateTopicRequestMessage(categoryId, topic, topicDescription);
                var createTopicResponse = await _messageBus.RequestAsync<CreateTopicRequestMessage, CreateTopicRequestMessageResponse>(createTopicMessage, context.CancellationToken);

                var sagaArticles = new List<ArticleJobItem>();
                foreach (var summary in articleList.Articles)
                {
                    var articleJob = new JobLog
                    {
                        UserId = userId,
                        JobType = AiJobType.ArticleGeneration,
                        AiModelId = job.AiModelId,
                        Status = AiGenerationJobStatus.Pending
                    };
                    _aiContext.JobLogs.Add(articleJob);

                    sagaArticles.Add(new ArticleJobItem(articleJob.Id, summary));
                }
                // Publish the saga
                await context.Publish(new StartArticleBatchGeneration
                {
                    CorrelationId = Guid.NewGuid(),
                    ParentJobId = jobId,
                    TopicId = createTopicResponse.TopicId,
                    UserId = userId,
                    AiModelId = job.AiModelId,
                    Articles = sagaArticles
                });

                job.Status = AiGenerationJobStatus.Completed;
                job.CompletedAt = DateTime.UtcNow;
                await _aiContext.SaveChangesAsync(context.CancellationToken);

                // // publish the GenerateArticleListMessage
                // var followUpMessage = new GenerateArticleListMessage(job.Id, topicId);
                // await _messageBus.PublishAsync(followUpMessage, context.CancellationToken);
                // return;
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