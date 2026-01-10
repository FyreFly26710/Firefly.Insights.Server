using System;
using System.Text.Json;
using Server.Ai.Api.Infrastructure.AiClients;
using Server.Ai.Api.Infrastructure.StateMachines;
using Server.Messages.Ais;
using Server.Messages.Contents;

namespace Server.Ai.Api.Infrastructure.Messaging;

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
        if (job == null)
        {
            _logger.LogCritical("Stopping the Job. Job log not found. JobLogId: {JobLogId}", message.JobId);
            return;
        }
        job.Status = AiGenerationJobStatus.Running;
        job.StartedAt = DateTime.UtcNow;
        await _aiContext.SaveChangesAsync(context.CancellationToken);
        try
        {
            // generate the article summary list
            var responseMessage = await _articleGenerationClient.GenerateArticleSummaryListAsync(message.JobId, job.AiModelId, message.ArticleCount, message.Topic, message.TopicDescription, message.Category, message.Prompt, context.CancellationToken);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (ResponseUtils.CodeBlockFound(responseMessage))
            {
                _logger.LogWarning("Code block found in response message, removing it. JobId: {JobId}", message.JobId);
                responseMessage = ResponseUtils.RemoveCodeBlock(responseMessage);
            }
            var articleList = JsonSerializer.Deserialize<GenerationArticleList>(responseMessage, options);

            if (articleList == null)
                throw new Exception("Parsing the article list response failed");

            // add topic id to article list
            var createTopicMessage = new CreateTopicRequestMessage(message.CategoryId, message.Topic, message.TopicDescription, message.TopicUrl);
            var createTopicResponse = await _messageBus.RequestAsync<CreateTopicRequestMessage, CreateTopicRequestMessageResponse>(createTopicMessage, context.CancellationToken);
            if (createTopicResponse.TopicId <= 0)
                throw new Exception("Creating the topic failed");

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
            _logger.LogInformation("Publishing the start article batch generation message. JobLogId: {JobLogId}, TopicId: {TopicId}", message.JobId, createTopicResponse.TopicId);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating article summary. JobLogId: {JobLogId}", message.JobId);
            job.Status = AiGenerationJobStatus.Failed;
            job.CompletedAt = DateTime.UtcNow;
            job.FailureReason = ex.Message;
            await _aiContext.SaveChangesAsync(context.CancellationToken);
        }
    }

}