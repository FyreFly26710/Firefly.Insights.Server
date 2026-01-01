using System;

namespace Server.Ai.Api.Models.Entities;

public class JobFollowUp : Entity
{
    public long JobLogId { get; set; }

    public JobFollowUpActionType ActionType { get; set; } = JobFollowUpActionType.Other;
    public string? Payload { get; set; } 
    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public JobLog? JobLog { get; set; }
}

public enum JobFollowUpActionType
{
    UpdateArticles,
    Other,
}