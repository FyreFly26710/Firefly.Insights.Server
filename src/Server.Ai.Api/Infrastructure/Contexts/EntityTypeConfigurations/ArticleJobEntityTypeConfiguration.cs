using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class ArticleJobEntityTypeConfiguration : IEntityTypeConfiguration<ArticleJob>
{
    public void Configure(EntityTypeBuilder<ArticleJob> builder)
    {
        builder.ToTable("ArticleJobs");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.JobLogId).IsRequired();
        builder.Property(e => e.ArticleId).IsRequired();

        builder.HasOne(e => e.JobLog).WithOne().HasForeignKey<ArticleJob>(e => e.JobLogId);
    }

}
