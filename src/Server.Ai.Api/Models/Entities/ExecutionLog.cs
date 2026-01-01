using System;

namespace Server.Ai.Api.Models.Entities;

public class ExecutionLog : Entity
{
    public long JobLogId { get; set; }

    public DateTime ExecutedAt { get; set; }
    public long ExecutionPayloadId { get; set; }

    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal Cost { get; set; }

    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }

    public TimeSpan Duration { get; set; }

    public ExecutionPayload? ExecutionPayload { get; set; }
    public JobLog? JobLog { get; set; }
}
