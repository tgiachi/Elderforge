using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Elderforge.Network.Data.Session;
using Elderforge.Network.Interfaces.Services;
using Serilog;

namespace Elderforge.Network.Services;

public class NetworkSessionService<TSessionData> : INetworkSessionService<TSessionData> where TSessionData : class
{
    private readonly ConcurrentDictionary<string, SessionObject<TSessionData>> _sessions = new();
    private readonly ILogger _logger = Log.ForContext<NetworkSessionService<SessionObject<TSessionData>>>();

    public List<string> GetSessionIds => _sessions.Keys.ToList();

    public SessionObject<TSessionData>? GetSessionObject(string sessionId)
    {
        return _sessions.GetValueOrDefault(sessionId);
    }

    public void AddSession(string sessionId, SessionObject<TSessionData> sessionObject)
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
