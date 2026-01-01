using System;

namespace Server.Ai.Api.Models.Entities;

public class AiChatConversation : Entity
{
    public long UserId { get; set; }

    public string Title { get; set; } = "New Chat";

    public long? AiModelId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastMessageAt { get; set; }

    public bool IsArchived { get; set; }
}
