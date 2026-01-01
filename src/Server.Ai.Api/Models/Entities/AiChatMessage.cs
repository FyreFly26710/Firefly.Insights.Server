using System;

namespace Server.Ai.Api.Models.Entities;

public class AiChatMessage : Entity
{
    public long AiChatConversationId { get; set; }

    public AiChatRole Role { get; set; } // System, User, Assistant

    public string Content { get; set; } = string.Empty;

    public long? AiModelId { get; set; }

    public int? InputTokens { get; set; }
    public int? OutputTokens { get; set; }
    public decimal? Cost { get; set; }

    public bool IsSystem { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AiChatConversation? Conversation { get; set; }
}

public enum AiChatRole
{
    System,
    User,
    Assistant,
}