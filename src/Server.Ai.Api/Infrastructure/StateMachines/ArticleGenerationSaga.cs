namespace Server.Ai.Api.Infrastructure.StateMachines;

public class ArticleGenerationSaga : MassTransitStateMachine<ArticleGenerationSagaState>
{
    public State GeneratingArticles { get; private set; }
    public State GeneratingSummary { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<StartArticleBatchGeneration> BatchStarted { get; private set; }
    public Event<ArticleContentGenerated> ArticleGenerated { get; private set; }
    public Event<ArticleContentGenerationFailed> ArticleFailed { get; private set; }

    public ArticleGenerationSaga(ILogger<ArticleGenerationSaga> logger)
    {

        InstanceState(x => x.CurrentState);

        Event(() => BatchStarted, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => ArticleGenerated, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => ArticleFailed, x => x.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(BatchStarted)
                .Then(ctx =>
                {
                    // Initialize State
                    ctx.Saga.ParentJobId = ctx.Message.ParentJobId;
                    ctx.Saga.TopicId = ctx.Message.TopicId;
                    ctx.Saga.TotalCount = ctx.Message.Articles.Count;
                    ctx.Saga.CreatedAt = DateTime.UtcNow;
                    ctx.Saga.CompletedCount = 0;
                    ctx.Saga.FailedCount = 0;
                })
                // --- FAN OUT ---
                // Iterate over the list and publish/send commands for each item
                .ThenAsync(async ctx =>
                        {
                            // Create the tasks for publishing each individual command
                            var tasks = ctx.Message.Articles.Select(articleItem => ctx.Publish(new GenerateArticleContentCommand
                            {
                                CorrelationId = ctx.Saga.CorrelationId,
                                JobLogId = articleItem.JobLogId,
                                UserId = ctx.Message.UserId,
                                AiModelId = ctx.Message.AiModelId,
                                TopicId = ctx.Message.TopicId,
                                ArticleSummary = articleItem.ArticleSummary
                            }));

                            await Task.WhenAll(tasks);
                        })
                .TransitionTo(GeneratingArticles)
        );
        During(GeneratingArticles,
            When(ArticleGenerated)
                .Then(ctx => ctx.Saga.CompletedCount++)
                // FAN IN
                .If(ctx => ctx.Saga.CompletedCount + ctx.Saga.FailedCount >= ctx.Saga.TotalCount,
                    // Batch is finished - we know at least ONE succeeded because we are in When(ArticleGenerated)
                    activity => activity.TransitionTo(GeneratingSummary)
                        .Publish(ctx => new GenerateTopicSummaryCommand(ctx.Saga.CorrelationId, ctx.Saga.ParentJobId, ctx.Saga.TopicId))
                ),

            When(ArticleFailed)
                .Then(ctx => ctx.Saga.FailedCount++)
                .If(ctx => ctx.Saga.CompletedCount + ctx.Saga.FailedCount >= ctx.Saga.TotalCount,
                    activity => activity.IfElse(ctx => ctx.Saga.CompletedCount > 0,
                        // Batch finished: Some failed, but at least one succeeded. Generate summary.
                        someSuccess => someSuccess.TransitionTo(GeneratingSummary)
                            .Publish(ctx => new GenerateTopicSummaryCommand(ctx.Saga.CorrelationId, ctx.Saga.ParentJobId, ctx.Saga.TopicId)),
                        // Batch finished: EVERY SINGLE ONE FAILED. 
                        allFailed => allFailed.TransitionTo(Failed)
                            .Then(ctx => logger.LogError("Job {ParentJobId} failed entirely.", ctx.Saga.ParentJobId))
                    )
                )
        );
    }
}
