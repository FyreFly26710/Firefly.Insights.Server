using System;

namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class AiProviderEntityTypeConfiguration : IEntityTypeConfiguration<AiProvider>   
{
    public void Configure(EntityTypeBuilder<AiProvider> builder)
    {
        builder.ToTable("AiProviders");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(128).IsRequired();
        builder.Property(e => e.ApiKey).HasMaxLength(1024).IsRequired();
    }
}
