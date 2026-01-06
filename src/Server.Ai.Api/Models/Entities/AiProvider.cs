namespace Server.Ai.Api.Models.Entities;
public partial class AiProvider : Entity
{
    public string Name { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}