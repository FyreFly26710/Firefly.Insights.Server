namespace Server.Contents.Api.Models.Requests;

public record ArticleUpdateRequest
(
    // Required
    long ArticleId,

    // Optional
    string? Title = null,
    string? Content = null,
    string? Description = null,
    string? ImageUrl = null,
    long? TopicId = null,
    bool? IsTopicSummary = null,
    int? SortNumber = null,
    bool? IsHidden = null
);
