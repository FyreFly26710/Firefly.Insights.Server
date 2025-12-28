using System;

namespace Server.Contents.Api.Models.Responses;

public class ArticleDto
{
    public long ArticleId { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string Description { get; set; } = "";

    public string ImageUrl { get; set; } = "";
    public long TopicId { get; set; }
    public string TopicName { get; set; } = "";
    public bool IsTopicSummary { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = "";
    public int SortNumber { get; set; }
    public bool IsHidden { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<string> Tags { get; set; } = [];
}
