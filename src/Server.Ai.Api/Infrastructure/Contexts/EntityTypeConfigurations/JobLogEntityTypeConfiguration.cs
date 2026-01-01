using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class JobLogEntityTypeConfiguration : IEntityTypeConfiguration<JobLog>
{
    public void Configure(EntityTypeBuilder<JobLog> builder)
    {
        builder.ToTable("JobLogs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.JobType).HasConversion<string>().HasMaxLength(64).IsRequired();
        builder.Property(e => e.AiModelId).IsRequired(false);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(64).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.StartedAt).IsRequired(false);
        builder.Property(e => e.CompletedAt).IsRequired(false);
        builder.Property(e => e.FailureReason).HasMaxLength(4096).IsRequired(false);
    
        builder.HasOne(e => e.AiModel).WithMany().HasForeignKey(e => e.AiModelId);
    }

}
