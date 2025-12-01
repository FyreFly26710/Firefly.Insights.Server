using Microsoft.EntityFrameworkCore;
using Server.Identity.Api.Models.Entities;

namespace Server.Identity.Api.Infrastructure;
public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Auth");
        ConfigureUser(builder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(u => u.UserAccount).HasMaxLength(32).IsRequired();
            entity.Property(u => u.UserPassword).HasMaxLength(32).IsRequired();
            entity.Property(u => u.UserEmail).HasMaxLength(128);
            entity.Property(u => u.UserName).HasMaxLength(32);
            entity.Property(u => u.UserAvatar).HasMaxLength(2048);
            entity.Property(u => u.UserProfile).HasMaxLength(2048);
            entity.Property(u => u.UserRole).HasMaxLength(32).IsRequired().HasDefaultValue("user");
        });
    }
}
