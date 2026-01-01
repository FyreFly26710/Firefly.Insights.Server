using System;
using Server.Common.Types;

namespace Server.Ai.Api.Models.Requests;

public record JobLogListRequest : PageInfo
{
    public long? UserId { get; init; }
    public long? AiModelId { get; init; }
    public AiJobType? JobType { get; init; }
    public AiGenerationJobStatus? Status { get; init; }
    public override bool IsAscending { get; init; } = false;
    public override string? SortField { get; init; } = nameof(JobLog.CreatedAt);
}
