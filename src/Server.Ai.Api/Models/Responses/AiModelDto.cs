using System;
using Server.Messages.Identities;

namespace Server.Ai.Api.Models.Responses;

public record AiModelDto
{
    public long AiModelId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public long AiProviderId { get; set; } = 0;
    public string Model { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public decimal InputPrice { get; set; } = 0;
    public decimal OutputPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    // public string ApiKey { get; set; } = string.Empty;

    // public UserTo User { get; set; } = new UserTo(0);
    // public string UserAvatar { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
