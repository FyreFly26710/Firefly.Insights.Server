using System;

namespace Server.Contents.Api.Models.Requests;

public class ArticleCreateRequest
{
    public required string Title { get; set; }
    public string Content { get; set; } = "";
    public string Description { get; set; } = "";

    public string ImageUrl { get; set; } = "";
    public required long TopicId { get; set; }
    public bool IsTopicSummary { get; set; } = false;
    public int? SortNumber { get; set; }
    public bool IsHidden { get; set; } = false;
}
