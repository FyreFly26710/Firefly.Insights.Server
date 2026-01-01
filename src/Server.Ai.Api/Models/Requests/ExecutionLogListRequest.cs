using System;

namespace Server.Ai.Api.Models.Requests;

public record ExecutionLogListRequest : PageInfo
{
    public long? UserId { get; init; }
    public long? AiModelId { get; init; }
    public bool? IsSuccessful { get; init; }
    public override bool IsAscending { get; init; } = false;
    public override string? SortField { get; init; } = nameof(ExecutionLog.ExecutedAt);
}
