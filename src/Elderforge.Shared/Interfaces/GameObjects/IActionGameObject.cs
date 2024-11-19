namespace Elderforge.Shared.Interfaces.GameObjects;

public interface IActionGameObject
{
    Task ActionAsync(IGameObject source);
}
