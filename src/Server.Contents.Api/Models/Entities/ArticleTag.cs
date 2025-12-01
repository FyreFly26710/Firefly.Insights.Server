using Server.Common.Types;

namespace Server.Contents.Api.Models.Entities;

public class ArticleTag : Entity
{
    public long ArticleMetaId { get; set; }
    public long TagId { get; set; }
    public ArticleMeta ArticleMeta { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
