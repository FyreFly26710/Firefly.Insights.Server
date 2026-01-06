using System;

namespace Server.Ai.Api.Models.Requests;

public class CreateAiModelRequest
{
    public long AiProviderId { get; set; }
    public string Model { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public decimal InputPrice { get; set; } = 0;
    public decimal OutputPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // public string ApiKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
