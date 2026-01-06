using System;

namespace Server.Ai.Api.Models.Requests;

public class AiProviderDto
{
    public long AiProviderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;

}
