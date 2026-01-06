namespace Server.Ai.Api.Infrastructure.Contexts.EntityTypeConfigurations;

public class AiModelEntityTypeConfiguration : IEntityTypeConfiguration<AiModel>
{
    public void Configure(EntityTypeBuilder<AiModel> builder)
    {
        builder.ToTable("AiModels");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.DisplayName).HasMaxLength(128).IsRequired();
        builder.Property(e => e.Avatar).HasMaxLength(1024).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(1024);
        builder.Property(e => e.Model).HasMaxLength(128).IsRequired();
        builder.Property(e => e.ModelId).HasMaxLength(128).IsRequired();
        builder.Property(e => e.InputPrice).HasPrecision(5, 2).IsRequired();
        builder.Property(e => e.OutputPrice).HasPrecision(5, 2).IsRequired();
        builder.Property(e => e.IsActive).IsRequired();
        builder.HasOne(e => e.AiProvider).WithMany().HasForeignKey(e => e.AiProviderId);
        // Use unique model as user account in identity api
        builder.HasIndex(e => e.ModelId).IsUnique();
    }
}
