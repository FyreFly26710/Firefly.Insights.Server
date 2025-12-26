using System;

namespace Server.Contents.Api.Models.Requests;

public class CategoryUpdateRequest
{
    public required long CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int? SortNumber { get; set; }
    public bool? IsHidden { get; set; }

}
