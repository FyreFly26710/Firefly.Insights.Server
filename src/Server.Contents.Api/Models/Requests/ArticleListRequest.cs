using System;
using Server.Common.Types;
using Server.Contents.Api.Models.Entities;

namespace Server.Contents.Api.Models.Requests;

public class ArticleListRequest : PageRequest
{
    public string? ArticleTitle { get; set; }
    public override SortOrder SortOrder { get; set; } = SortOrder.desc;
    public override string? SortField { get; set; } = nameof(ArticleMeta.CreatedAt);
}
