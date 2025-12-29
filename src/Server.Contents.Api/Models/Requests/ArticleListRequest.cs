using System;
using Server.Common.Types;
using Server.Contents.Api.Models.Entities;

namespace Server.Contents.Api.Models.Requests;

public record ArticleListRequest : PageInfo
{
    public string? ArticleTitle { get; init; }
    public long? TopicId { get; init; }
    public bool? IsTopicSummary { get; init; }
    public bool? IsHidden { get; init; }
    public long? UserId { get; init; }
    public List<string>? Tags { get; init; }
    public override bool IsAscending { get; init; } = false;
    public override string? SortField { get; init; } = nameof(ArticleMeta.CreatedAt);
}
