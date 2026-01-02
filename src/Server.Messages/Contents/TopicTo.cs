using System;

namespace Server.Messages.Contents;

public record TopicTo
{
    public long TopicId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string CategoryDescription { get; set; } = "";

    // public string ImageUrl { get; set; } = "";
    // public int SortNumber { get; set; }
    // public bool IsHidden { get; set; }

    public List<TopicArticleTo>? TopicArticles { get; set; } = null;
}

public record TopicArticleTo
{
    public long ArticleId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int SortNumber { get; set; }
    public List<string> Tags { get; set; } = [];
}