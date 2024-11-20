namespace Elderforge.Shared.Interfaces;

public interface IPlayerGameObject : IGameObject
{
    string SessionId { get; set; }
}
