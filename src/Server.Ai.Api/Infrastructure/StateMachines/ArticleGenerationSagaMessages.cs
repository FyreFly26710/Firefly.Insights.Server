using System;

namespace Server.Ai.Api.Infrastructure.StateMachines;


// 1. Trigger: Starts the Saga (Contains the list to fan-out)
public record StartArticleBatchGeneration
{
    public Guid CorrelationId { get; init; } // The Saga ID
    public long ParentJobId { get; init; }
    public long TopicId { get; init; }
    public long UserId { get; init; }
    public long AiModelId { get; init; }

    // The payload to fan-out
    public List<ArticleJobItem> Articles { get; init; } = new();
}

public record ArticleJobItem(long JobLogId, GenerationArticleSummary ArticleSummary);

// 2. Command: Sent BY the Saga TO the Worker
public record GenerateArticleContentCommand
{
    public Guid CorrelationId { get; init; } // Links back to Saga
    public long JobLogId { get; init; }
    public long ParentJobLogId { get; init; }
    public long UserId { get; init; }
    public long AiModelId { get; init; }
    public long TopicId { get; init; }
    public GenerationArticleSummary ArticleSummary { get; init; }
}

// 3. Event: Sent BY the Worker BACK to the Saga (Fan-In)
public record ArticleContentGenerated(Guid CorrelationId, long JobLogId);

public record ArticleContentGenerationFailed(Guid CorrelationId, long JobLogId, string Reason);

// 4. Command: Final step (Topic Summary)
public record GenerateTopicSummaryCommand(Guid CorrelationId, long ParentJobId, long TopicId);