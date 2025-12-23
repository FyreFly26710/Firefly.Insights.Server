using System;

namespace Server.Contents.Api.Models.Requests;

public class CategoryCreateRequest
{
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    // public int SortNumber { get; set; }
    public bool IsHidden { get; set; } = false;
}
