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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
