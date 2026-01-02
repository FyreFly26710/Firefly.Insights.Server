using System;

namespace Server.Ai.Api.Models.Entities;

public class ArticleGenerationSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public long ParentJobId { get; set; }
    public long TopicId { get; set; }

    // Tracking Counters
    public int TotalCount { get; set; }
    public int CompletedCount { get; set; }
    public int FailedCount { get; set; }

    public DateTime CreatedAt { get; set; }
}