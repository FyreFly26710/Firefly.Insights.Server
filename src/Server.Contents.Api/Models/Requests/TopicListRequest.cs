using System;

namespace Server.Contents.Api.Models.Requests;

public record TopicListRequest : PageInfo
{
    public string? TopicName { get; init; }
    public long? CategoryId { get; init; }
    public bool? IsHidden { get; init; }
    public override bool IsAscending { get; init; } = false;
    public override string? SortField { get; init; } = nameof(Topic.CreatedAt);
}
