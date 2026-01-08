using Server.Common.Extensions;

namespace Server.Contents.Api.Infrastructure;

public class ContentsContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleMeta> ArticleMetas { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Topic> Topics { get; set; }

    public ContentsContext(DbContextOptions<ContentsContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("insights");
        builder.ApplySoftDeleteQueryFilter();

        ConfigureArticle(builder);
        ConfigureArticleMeta(builder);
        ConfigureArticleTag(builder);
        ConfigureCategory(builder);
        ConfigureTag(builder);
        ConfigureTopic(builder);
    }

    private void ConfigureTopic(ModelBuilder builder)
    {
        builder.Entity<Topic>(entity =>
        {
            entity.ToTable("Topics");
            entity.Property(t => t.Id).ValueGeneratedNever();
            entity.Property(t => t.Name).HasMaxLength(256).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(1024).IsRequired();
            // entity.Property(t => t.ImageUrl).HasMaxLength(512);

            entity.HasOne(t => t.Category).WithMany(c => c.Topics).HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.NoAction);
        });
    }

    private void ConfigureTag(ModelBuilder builder)
    {
        builder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");
            entity.Property(t => t.Id).ValueGeneratedNever();
            entity.Property(t => t.Name).HasMaxLength(64).IsRequired();
            entity.Property(t => t.Type).HasConversion<string>().HasMaxLength(64).IsRequired();
            entity.HasIndex(t => new { t.Name, t.Type }).IsUnique();
        });
    }

    private void ConfigureCategory(ModelBuilder builder)
    {
        builder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(c => c.Id).ValueGeneratedNever();
            entity.Property(c => c.Name).HasMaxLength(256).IsRequired();
            entity.Property(c => c.Description).HasMaxLength(512).IsRequired();
            // entity.Property(c => c.ImageUrl).HasMaxLength(512).IsRequired();
        });
    }

    private void ConfigureArticleTag(ModelBuilder builder)
    {
        builder.Entity<ArticleTag>(entity =>
        {
            entity.ToTable("ArticleTags");
            entity.Property(at => at.Id).ValueGeneratedNever();
            entity.HasOne(at => at.Tag).WithMany().HasForeignKey(at => at.TagId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(at => at.ArticleMeta).WithMany(am => am.ArticleTags).HasForeignKey(at => at.ArticleMetaId).OnDelete(DeleteBehavior.NoAction);
        });
    }

    private void ConfigureArticleMeta(ModelBuilder builder)
    {
        builder.Entity<ArticleMeta>(entity =>
        {
            entity.ToTable("ArticleMetas");
            entity.Property(am => am.Id).ValueGeneratedNever();
            // entity.Property(am => am.ImageUrl).HasMaxLength(512).IsRequired();

            entity.HasOne(am => am.Article).WithOne(a => a.ArticleMeta).HasForeignKey<ArticleMeta>(a => a.ArticleId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(am => am.Topic).WithMany(a => a.ArticleMetas).HasForeignKey(a => a.TopicId).OnDelete(DeleteBehavior.NoAction);
        });
    }

    private void ConfigureArticle(ModelBuilder builder)
    {
        builder.Entity<Article>(entity =>
        {
            entity.ToTable("Articles");
            entity.Property(a => a.Id).ValueGeneratedNever();
            entity.Property(a => a.Title).HasMaxLength(256).IsRequired();
            entity.Property(a => a.Description).HasMaxLength(1024).IsRequired();
            entity.Property(a => a.Content).IsRequired();
        });
    }



}
