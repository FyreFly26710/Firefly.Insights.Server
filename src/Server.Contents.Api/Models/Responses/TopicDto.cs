using System;

namespace Server.Contents.Api.Models.Responses;

public class TopicDto
{
    public long TopicId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }

    public ICollection<TopicArticleDto>? TopicArticles { get; set; } = null;
}

public class TopicArticleDto
{
    public long ArticleId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }
    public List<string> Tags { get; set; } = [];
}