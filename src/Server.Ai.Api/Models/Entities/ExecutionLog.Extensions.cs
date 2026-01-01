using System;

namespace Server.Ai.Api.Models.Entities;

public static class ExecutionLogExtensions
{
    public static ExecutionLogDto ToExecutionLogDto(this ExecutionLog executionLog, JobLogDto jobLog) => new ExecutionLogDto()
    {
        ExecutionLogId = executionLog.Id,
        JobLog = jobLog,
        ExecutedAt = executionLog.ExecutedAt,
        ExecutionPayloadId = executionLog.ExecutionPayloadId,
        InputTokens = executionLog.InputTokens,
        OutputTokens = executionLog.OutputTokens,
        ReasoningTokens = executionLog.ReasoningTokens,
        Cost = executionLog.Cost,
        IsSuccessful = executionLog.IsSuccessful,
        ErrorMessage = executionLog.ErrorMessage,
        Duration = executionLog.Duration,
    };
}
