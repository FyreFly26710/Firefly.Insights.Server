using Server.Common.Types;

namespace Server.Contents.Api.Models.Entities;

public partial class Topic : AuditableEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public long CategoryId { get; set; }
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }

    public ICollection<ArticleMeta> ArticleMetas { get; set; } = [];
    public Category Category { get; set; } = null!;
}
