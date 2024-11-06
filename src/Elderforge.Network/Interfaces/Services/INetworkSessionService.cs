namespace Elderforge.Network.Interfaces.Services;

public interface INetworkSessionService<TSessionObject> where TSessionObject : class
{
    TSessionObject? GetSessionObject(string sessionId);

    void AddSession(string sessionId, TSessionObject sessionObject);

    void RemoveSession(string sessionId);
}
