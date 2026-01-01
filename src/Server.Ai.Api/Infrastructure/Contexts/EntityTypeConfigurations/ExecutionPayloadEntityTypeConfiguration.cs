using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class ExecutionPayloadEntityTypeConfiguration : IEntityTypeConfiguration<ExecutionPayload>
{
    public void Configure(EntityTypeBuilder<ExecutionPayload> builder)
    {
        builder.ToTable("ExecutionPayloads");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.RequestJson).HasColumnType("text").IsRequired();
        builder.Property(e => e.ResponseJson).HasColumnType("text").IsRequired(false);
    }
}
