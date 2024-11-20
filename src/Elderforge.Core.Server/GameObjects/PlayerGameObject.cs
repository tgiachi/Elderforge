using Elderforge.Core.Server.GameObjects.Base;
using Elderforge.Shared.Interfaces;
using Elderforge.Shared.Types;

namespace Elderforge.Core.Server.GameObjects;

public class PlayerGameObject : AbstractGameObject, IPlayerGameObject
{
    public Guid PlayerId { get; set; }
    public string SessionId { get; set; }
    public PlayerGameObject() : base(GameObjectType.Player)
    {
    }


}
