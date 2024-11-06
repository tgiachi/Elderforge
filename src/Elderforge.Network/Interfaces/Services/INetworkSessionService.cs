using Elderforge.Network.Data.Session;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkSessionService<TSessionData> where TSessionData : class
{
    SessionObject<TSessionData>? GetSessionObject(string sessionId);
    void AddSession(string sessionId, SessionObject<TSessionData> sessionObject);

    void RemoveSession(string sessionId);
}
