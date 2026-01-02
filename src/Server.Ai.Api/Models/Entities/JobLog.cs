using System;

namespace Server.Ai.Api.Models.Entities;
public class JobLog : Entity
{
    public long UserId { get; set; }
    public AiJobType JobType { get; set; } = AiJobType.Other;

    public long AiModelId { get; set; }

    public AiGenerationJobStatus Status { get; set; } = AiGenerationJobStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Errors before triggering the job, eg. Invalid modelId
    public string? FailureReason { get; set; }

    public ExecutionLog? ExecutionLog { get; set; }
    public AiModel? AiModel { get; set; }
    public ICollection<JobFollowUp>? FollowUps { get; set; } = null;

}
public enum AiGenerationJobStatus
{
    Pending,
    Running,
    Completed,
    Failed,
}
public enum AiJobType
{
    ArticleSummary,
    ArticleGeneration,
    TopicSummaryGeneration,
    Other
}