namespace Server.Contents.Api.Models.Requests;

public record TopicUpdateRequest
(
    // Required
    long TopicId,

    // Optional
    string? Name = null,
    string? Description = null,
    long? CategoryId = null,
    string? ImageUrl = null,
    int? SortNumber = null,
    bool? IsHidden = null
);
