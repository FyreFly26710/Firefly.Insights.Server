using System;

namespace Server.Ai.Api.Models.Requests;

public class UpdateAiModelRequest
{
    // public string? Provider { get; set; }
    public long? AiProviderId { get; set; }
    public string? Model { get; set; }
    public string? ModelId { get; set; }
    public decimal? InputPrice { get; set; }
    public decimal? OutputPrice { get; set; }
    public bool? IsActive { get; set; }

    // public string? ApiKey { get; set; }

    // public long? ModelUserId { get; set; }
    public string? DisplayName { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
}