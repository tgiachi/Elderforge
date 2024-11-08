using System.Collections.Generic;
using Elderforge.Network.Data.Session;
using Elderforge.Network.Interfaces.Sessions;

namespace Elderforge.Network.Interfaces.Services;

public interface INetworkSessionService
{
    List<string> GetSessionIds { get; }
    ISessionObject? GetSessionObject(string sessionId);
    void AddSession(string sessionId, ISessionObject sessionObject);
    void RemoveSession(string sessionId);
}
