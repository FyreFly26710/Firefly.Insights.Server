using Server.Common.Types;

namespace Server.Contents.Api.Models.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public int IsHidden { get; set; }

    public ICollection<Topic> Topics { get; set; } = [];
}
