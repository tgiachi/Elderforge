using System.Collections.Concurrent;
using System.Collections.Generic;
using Elderforge.Network.Interfaces.Services;
using Serilog;

namespace Elderforge.Network.Services;

public class NetworkSessionService<TSessionObject> : INetworkSessionService<TSessionObject> where TSessionObject : class
{
    private readonly ConcurrentDictionary<string, TSessionObject> _sessions = new();
    private readonly ILogger _logger = Log.ForContext<NetworkSessionService<TSessionObject>>();

    public TSessionObject? GetSessionObject(string sessionId) => _sessions.GetValueOrDefault(sessionId);

    public void AddSession(string sessionId, TSessionObject sessionObject)
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
