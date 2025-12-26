using System;

namespace Server.Contents.Api.Models.Requests;

public record TopicCreateRequest
(
    // Required
    string Name,
    long CategoryId,

    // Optional
    string Description = "",
    string ImageUrl = "",
    int SortNumber = 0,
    bool IsHidden = false
);
