using System;

namespace Server.Ai.Api.Models.Entities;

public class ExecutionLog : Entity
{
    public ExecutionLog(){}
    public ExecutionLog(long jobLogId, DateTime executedAt, string errorMessage, string prompt)
    {
        JobLogId = jobLogId;
        ExecutedAt = executedAt;
        ErrorMessage = errorMessage;
        IsSuccessful = false;
        Duration = TimeSpan.Zero;
        ExecutionPayload = new ExecutionPayload
        {
            Prompt = prompt,
            Response = null
        };
    }
    public long JobLogId { get; set; }

    public DateTime ExecutedAt { get; set; }
    public long ExecutionPayloadId { get; set; }

    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int ReasoningTokens { get; set; }
    public decimal Cost { get; set; }

    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }

    public TimeSpan Duration { get; set; }

    public ExecutionPayload? ExecutionPayload { get; set; }
    public JobLog? JobLog { get; set; }
}
