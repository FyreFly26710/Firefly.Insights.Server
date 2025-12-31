using System;

namespace Server.Contents.Api.Models.Requests;

public record CategoryUpdateRequest
(
    // Required
    long CategoryId,

    // Optional
    string? Name = null,
    string? Description = null,
    string? ImageUrl = null,
    int? SortNumber = null,
    bool? IsHidden = null,
    List<CategoryUpdateRequestTopic>? Topics = null
);
public record CategoryUpdateRequestTopic
(
    long TopicId,
    string Name, // Not used
    int SortNumber,
    bool IsHidden
);