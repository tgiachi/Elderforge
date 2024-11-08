using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Elderforge.Network.Data.Session;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Interfaces.Sessions;
using Serilog;

namespace Elderforge.Network.Services;

public class NetworkSessionService : INetworkSessionService
{
    private readonly ConcurrentDictionary<string, ISessionObject> _sessions = new();
    private readonly ILogger _logger = Log.ForContext<NetworkSessionService>();

    public List<string> GetSessionIds => _sessions.Keys.ToList();

    public ISessionObject? GetSessionObject(string sessionId)
    {
        return _sessions.GetValueOrDefault(sessionId);
    }

    public void AddSession(string sessionId, ISessionObject sessionObject)
    {
        _logger.Debug("Adding session {sessionId}", sessionId);
        _sessions.TryAdd(sessionId, sessionObject);
    }

    public void RemoveSession(string sessionId)
    {
        _logger.Debug("Removing session {sessionId}", sessionId);
        _sessions.TryRemove(sessionId, out _);
    }
}
