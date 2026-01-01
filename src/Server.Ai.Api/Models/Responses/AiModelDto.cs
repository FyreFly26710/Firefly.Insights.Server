using System;

namespace Server.Ai.Api.Models.Responses;

public record AiModelDto
{
    public long AiModelId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public decimal InputPrice { get; set; } = 0;
    public decimal OutputPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;

}
