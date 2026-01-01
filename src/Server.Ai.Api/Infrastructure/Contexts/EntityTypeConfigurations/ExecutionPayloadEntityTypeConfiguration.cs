using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class ExecutionPayloadEntityTypeConfiguration : IEntityTypeConfiguration<ExecutionPayload>
{
    public void Configure(EntityTypeBuilder<ExecutionPayload> builder)
    {
        builder.ToTable("ExecutionPayloads");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Prompt).HasColumnType("text").IsRequired();
        builder.Property(e => e.Response).HasColumnType("text").IsRequired(false);
    }
}
