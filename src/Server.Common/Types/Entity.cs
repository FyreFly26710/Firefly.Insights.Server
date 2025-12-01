using Server.Common.Utils;

namespace Server.Common.Types;
/// <summary>
/// Base entity for domain models
/// </summary>
public abstract class Entity
{
    public long Id { get; set; } = SnowflakeId.GenerateId();

}
public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
