using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class AiChatConversationEntityTypeConfiguration : IEntityTypeConfiguration<AiChatConversation>
{
    public void Configure(EntityTypeBuilder<AiChatConversation> builder)
    {
        builder.ToTable("AiChatConversations");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Title).HasMaxLength(128).IsRequired();
        builder.Property(e => e.AiModelId).IsRequired(false);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.LastMessageAt).IsRequired(false);
        builder.Property(e => e.IsArchived).IsRequired();
    }

}
