using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class JobFollowUpEntityTypeConfiguration : IEntityTypeConfiguration<JobFollowUp>
{
    public void Configure(EntityTypeBuilder<JobFollowUp> builder)
    {
        builder.ToTable("JobFollowUps");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.JobLogId).IsRequired();
        builder.Property(e => e.ParentJobLogId).IsRequired();

        builder.HasOne(e => e.JobLog).WithMany(e => e.FollowUps).HasForeignKey(e => e.JobLogId);
    }

}
