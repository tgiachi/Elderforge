namespace Elderforge.Shared.Interfaces;

public interface IPlayerGameObject : IGameObject
{
    Guid PlayerId { get; set; }
    string SessionId { get; set; }
}
