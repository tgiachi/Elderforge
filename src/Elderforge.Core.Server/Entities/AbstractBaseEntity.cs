using Elderforge.Core.Server.Interfaces.Entities;

namespace Elderforge.Core.Server.Entities;

public abstract class AbstractBaseEntity : IBaseDbEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
