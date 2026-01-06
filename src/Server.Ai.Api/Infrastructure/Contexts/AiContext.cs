using Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;
using Server.Common.Extensions;

namespace Server.Ai.Api.Infrastructure.Contexts;

public class AiContext : DbContext
{
    public DbSet<AiModel> AiModels { get; set; }
    public DbSet<AiProvider> AiProviders { get; set; }
    public DbSet<JobLog> JobLogs { get; set; }
    public DbSet<ExecutionLog> ExecutionLogs { get; set; }
    public DbSet<JobFollowUp> JobFollowUps { get; set; }
    public DbSet<ExecutionPayload> ExecutionPayloads { get; set; }
    public DbSet<AiChatConversation> ChatConversations { get; set; }
    public DbSet<AiChatMessage> ChatMessages { get; set; }
    public DbSet<ArticleJob> ArticleJobs { get; set; }
    public AiContext(DbContextOptions<AiContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplySoftDeleteQueryFilter();
        builder.HasDefaultSchema("insights");
        builder.ApplyConfiguration(new AiModelEntityTypeConfiguration());
        builder.ApplyConfiguration(new AiProviderEntityTypeConfiguration());
        builder.ApplyConfiguration(new JobLogEntityTypeConfiguration());
        builder.ApplyConfiguration(new ExecutionLogEntityTypeConfiguration());
        builder.ApplyConfiguration(new JobFollowUpEntityTypeConfiguration());
        builder.ApplyConfiguration(new ExecutionPayloadEntityTypeConfiguration());
        builder.ApplyConfiguration(new AiChatConversationEntityTypeConfiguration());
        builder.ApplyConfiguration(new AiChatMessageEntityTypeConfiguration());
        builder.ApplyConfiguration(new ArticleJobEntityTypeConfiguration());

    }
}
