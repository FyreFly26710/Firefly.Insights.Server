using System;

namespace Server.Ai.Api.Models.Responses;

public record ExecutionLogDto
{
    public long ExecutionLogId { get; set; }
    public JobLogDto JobLog { get; set; } = new();

    public DateTime ExecutedAt { get; set; }
    public long ExecutionPayloadId { get; set; }

    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int ReasoningTokens { get; set; }
    public decimal Cost { get; set; }

    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }

    public TimeSpan Duration { get; set; }
}
