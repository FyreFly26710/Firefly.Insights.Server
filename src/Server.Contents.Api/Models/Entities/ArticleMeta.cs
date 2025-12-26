using Server.Common.Types;

namespace Server.Contents.Api.Models.Entities;

public class ArticleMeta : AuditableEntity
{
    public long ArticleId { get; set; }
    public string ImageUrl { get; set; } = "";

    public long TopicId { get; set; }
    public bool IsTopicSummary { get; set; }
    public long UserId { get; set; }
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }

    public Article Article { get; set; } = null!;
    public Topic Topic { get; set; } = null!;
    public ICollection<ArticleTag> ArticleTags { get; set; } = [];
}
