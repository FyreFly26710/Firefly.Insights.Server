using System;

namespace Server.Contents.Api.Models.Requests;

public record CategoryCreateRequest(
    // Required
    string Name,

    // Optional
    string Description = "",
    string ImageUrl = "",
    int SortNumber = 0,
    bool IsHidden = false
);