using Server.Common.Types;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Models.Entities;

public partial class Article : Entity
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string Description { get; set; } = "";

    public ArticleMeta ArticleMeta { get; set; } = null!;

}