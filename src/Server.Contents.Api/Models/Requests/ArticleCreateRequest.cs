namespace Server.Contents.Api.Models.Requests;

public record ArticleCreateRequest
(
    // Required
    string Title,
    long TopicId,

    // Optional
    string Content = "",
    string Description = "",
    string ImageUrl = "",
    bool IsTopicSummary = false,
    int SortNumber = 0,
    bool IsHidden = false,
    List<string>? Tags = null
);