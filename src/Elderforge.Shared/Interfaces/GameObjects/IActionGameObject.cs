using Elderforge.Shared.Types;

namespace Elderforge.Shared.Interfaces.GameObjects;

public interface IActionGameObject
{
    Task ActionAsync(GameObjectActionType actionType, IGameObject source);
}
