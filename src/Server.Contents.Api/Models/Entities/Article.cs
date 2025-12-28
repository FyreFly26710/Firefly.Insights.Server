namespace Server.Contents.Api.Models.Entities;

public class Article : AuditableEntity
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string Description { get; set; } = "";

    public ArticleMeta ArticleMeta { get; set; } = null!;

}