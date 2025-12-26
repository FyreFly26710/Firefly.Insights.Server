namespace Server.Contents.Api.Models.Requests;

public class ArticleUpdateRequest
{
    public required long ArticleId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public long? TopicId { get; set; }
    public bool? IsTopicSummary { get; set; }
    public int? SortNumber { get; set; }
    public bool? IsHidden { get; set; }
}
