using System;

namespace Server.Contents.Api.Models.Responses;

public class CategoryDto
{
    public long CategoryId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }

    public ICollection<CategoryTopicDto>? CategoryTopics { get; set; } = null;
}

public class CategoryTopicDto
{
    public long TopicId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }
}