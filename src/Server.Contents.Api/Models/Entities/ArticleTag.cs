namespace Server.Contents.Api.Models.Entities;

public class ArticleTag : AuditableEntity
{
    public long ArticleMetaId { get; set; }
    public long TagId { get; set; }
    public ArticleMeta ArticleMeta { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
