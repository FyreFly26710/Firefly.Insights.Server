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
        builder.Property(e => e.ActionType).HasConversion<string>().HasMaxLength(64).IsRequired();
        builder.Property(e => e.Payload).HasColumnType("text").IsRequired(false);
        builder.Property(e => e.IsSuccessful).IsRequired();
        builder.Property(e => e.ErrorMessage).HasMaxLength(4096).IsRequired(false);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.CompletedAt).IsRequired(false);

        builder.HasOne(e => e.JobLog).WithOne().HasForeignKey<JobFollowUp>(e => e.JobLogId);
    }

}
