using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class ExecutionLogEntityTypeConfiguration : IEntityTypeConfiguration<ExecutionLog>
{
    public void Configure(EntityTypeBuilder<ExecutionLog> builder)
    {
        builder.ToTable("ExecutionLogs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.JobLogId).IsRequired();
        builder.Property(e => e.ExecutedAt).IsRequired();
        builder.Property(e => e.ExecutionPayloadId).IsRequired();
        builder.Property(e => e.InputTokens).IsRequired();
        builder.Property(e => e.OutputTokens).IsRequired();
        builder.Property(e => e.Cost).HasPrecision(18, 10).IsRequired();
        builder.Property(e => e.IsSuccessful).IsRequired();
        builder.Property(e => e.ErrorMessage).HasMaxLength(4096).IsRequired(false);
        builder.Property(e => e.Duration).IsRequired();

        builder.HasOne(e => e.ExecutionPayload).WithMany().HasForeignKey(e => e.ExecutionPayloadId);
        builder.HasOne(e => e.JobLog).WithOne(e => e.ExecutionLog).HasForeignKey<ExecutionLog>(e => e.JobLogId);
    }

}
