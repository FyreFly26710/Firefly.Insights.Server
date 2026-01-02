// using System;
// using System.Text.Json;
// using Server.Messages.Contents;

// namespace Server.Ai.Api.Infrastructure.Messaging;

// public class GenerateArticleListMessageConsumer(
//     ILogger<GenerateArticleSummaryConsumer> _logger,
//     IArticleGenerationClient _articleGenerationClient,
//     AiContext _aiContext,
//     IMessageBus _messageBus)
//     : IConsumer<GenerateArticleListMessage>
// {
//     public async Task Consume(ConsumeContext<GenerateArticleListMessage> context)
//     {
//         var message = context.Message;
//         var query = _aiContext.JobLogs.AsQueryable().Include(j => j.AiModel);
//         var parentJob = await query.Where(j => j.Id == message.ParentJobLogId).FirstOrDefaultAsync(context.CancellationToken);
//         if (parentJob == null) return; // do nothing

//         var articleList = await GetArticleList(message.ParentJobLogId, context.CancellationToken);
//         if (articleList == null) return; // do nothing, already handled above

//         // Add topic
//         var topicRequestMessage = new GetTopicRequestMessage(message.TopicId);
//         var topicResponse = await _messageBus.RequestAsync<GetTopicRequestMessage, GetTopicRequestMessageResponse>(topicRequestMessage, context.CancellationToken);
//         // build dictionary
//         var jobArticleDictionary = new Dictionary<JobLog, GenerationArticleSummary>();
//         var jobs = new List<JobLog>();

//         ///
//         /// Create article generation job logs
//         /// 
//         foreach (var article in articleList.Articles)
//         {
//             var job = new JobLog()
//             {
//                 UserId = parentJob.UserId,
//                 JobType = AiJobType.ArticleGeneration,
//                 AiModelId = parentJob.AiModelId,
//                 Status = AiGenerationJobStatus.Pending,
//                 CreatedAt = DateTime.UtcNow,
//             };
//             jobs.Add(job);
//             jobArticleDictionary.Add(job, article);
//         }
//         _aiContext.JobLogs.AddRange(jobs);
//         await _aiContext.SaveChangesAsync(context.CancellationToken);

//         ///
//         /// Execute article generation jobs
//         /// Fan out the article generation jobs
//         /// Fan in in 
//         /// 
//         foreach (var jobArticle in jobArticleDictionary)
//         {
//             var generateArticleContentMessage = new GenerateArticleContentMessage(jobArticle.Key.Id, 0, jobArticle.Key.AiModelId, jobArticle.Value, topicResponse.Topic);
//             await _messageBus.PublishAsync(generateArticleContentMessage, context.CancellationToken);
//         }

//         ///
//         /// Create & Execute topic summary job
//         /// 
//         var topicSummaryJob = new JobLog()
//         {
//             UserId = parentJob.UserId,
//             JobType = AiJobType.TopicSummaryGeneration,
//             AiModelId = parentJob.AiModelId,
//             Status = AiGenerationJobStatus.Running,
//             CreatedAt = DateTime.UtcNow,
//             StartedAt = DateTime.UtcNow,
//         };
//         _aiContext.JobLogs.Add(topicSummaryJob);
//         await _aiContext.SaveChangesAsync(context.CancellationToken);

//         try
//         {
//             // get topic
//             var topicSummaryRequestMessage = new GetTopicRequestMessage(message.TopicId, true);
//             var topicSummaryResponse = await _messageBus.RequestAsync<GetTopicRequestMessage, GetTopicRequestMessageResponse>(topicSummaryRequestMessage, context.CancellationToken);

//             // Generate topic summary
//             var articleContent = await _articleGenerationClient.GenerateTopicSummaryAsync(topicSummaryJob.Id, topicSummaryJob.AiModelId, topicSummaryResponse.Topic, context.CancellationToken);
//             // Create article
//             var createArticleRequestMessage = new CreateArticleRequestMessage("Summary Article", message.TopicId, 0, articleContent);
//             await _messageBus.RequestAsync<CreateArticleRequestMessage, CreateArticleRequestMessageResponse>(createArticleRequestMessage, context.CancellationToken);
//             topicSummaryJob.Status = AiGenerationJobStatus.Completed;
//             topicSummaryJob.CompletedAt = DateTime.UtcNow;
//             await _aiContext.SaveChangesAsync(context.CancellationToken);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Error generating topic summary");
//             topicSummaryJob.Status = AiGenerationJobStatus.Failed;
//             topicSummaryJob.CompletedAt = DateTime.UtcNow;
//             topicSummaryJob.FailureReason = ex.Message;
//             await _aiContext.SaveChangesAsync(context.CancellationToken);
//         }
//     }




//     // this should not throw exceptions, most of cases should be handled in the previous message 
//     private async Task<GenerationArticleList?> GetArticleList(long parentJobLogId, CancellationToken cancellationToken = default)
//     {
//         var query = _aiContext.JobLogs.AsQueryable();
//         query = query.Include(j => j.ExecutionLog).ThenInclude(e => e.ExecutionPayload);
//         var job = await query.Where(j => j.Id == parentJobLogId).FirstOrDefaultAsync(cancellationToken);
//         if (job == null) return null; // do nothing
//         try
//         {
//             var payload = job.ExecutionLog?.ExecutionPayload;
//             if (payload == null)
//             {
//                 _logger.LogError("Execution payload not found: {JobId}", parentJobLogId);
//                 throw new ExceptionNotFound($"Execution payload not found: {parentJobLogId}");
//             }
//             var articleList = JsonSerializer.Deserialize<GenerationArticleList>(payload.Response);
//             if (articleList == null || string.IsNullOrEmpty(articleList.AiMessage))
//             {
//                 _logger.LogError("Invalid response message: {Response}", payload.Response);
//                 throw new ExceptionBadRequest($"Invalid response message: {payload.Response}");
//             }
//             return articleList;
//         }
//         catch (Exception ex)
//         {
//             job.Status = AiGenerationJobStatus.Failed;
//             job.CompletedAt = DateTime.UtcNow;
//             job.FailureReason = ex.Message;
//             await _aiContext.SaveChangesAsync(cancellationToken);
//             return null;
//         }
//     }
// }