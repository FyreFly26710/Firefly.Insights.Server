using System;

namespace Server.Ai.Api.Models.Responses;

public record JobLogDto
{
    public long JobLogId { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public AiModelDto AiModel { get; set; } = new();

    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
