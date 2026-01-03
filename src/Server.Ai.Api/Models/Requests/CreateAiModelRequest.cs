using System;

namespace Server.Ai.Api.Models.Requests;

public class CreateAiModelRequest
{
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public decimal InputPrice { get; set; } = 0;
    public decimal OutputPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public string ApiKey { get; set; } = string.Empty;

    public string AgentName { get; set; } = string.Empty;
    public string AgentAvatarUrl { get; set; } = string.Empty;
}
