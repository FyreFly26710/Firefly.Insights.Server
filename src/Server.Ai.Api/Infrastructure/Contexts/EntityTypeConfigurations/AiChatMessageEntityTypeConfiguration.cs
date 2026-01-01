using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class AiChatMessageEntityTypeConfiguration : IEntityTypeConfiguration<AiChatMessage>
{
    public void Configure(EntityTypeBuilder<AiChatMessage> builder)
    {
        builder.ToTable("AiChatMessages");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.AiChatConversationId).IsRequired();
        builder.Property(e => e.Role).HasConversion<string>().HasMaxLength(16).IsRequired();
        builder.Property(e => e.Content).HasColumnType("text").IsRequired();
        builder.Property(e => e.AiModelId).IsRequired(false);
        builder.Property(e => e.InputTokens).IsRequired(false);
        builder.Property(e => e.OutputTokens).IsRequired(false);
        builder.Property(e => e.Cost).HasPrecision(18, 10).IsRequired(false);

        builder.HasOne(e => e.Conversation).WithMany().HasForeignKey(e => e.AiChatConversationId);
    }

}
