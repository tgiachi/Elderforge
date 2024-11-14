using System.ComponentModel.DataAnnotations.Schema;
using Elderforge.Core.Server.Entities;

namespace Elderforge.Entities.Database;

[Table("users")]
public class UserEntity : AbstractBaseEntity
{
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public bool IsAdmin { get; set; }
}
