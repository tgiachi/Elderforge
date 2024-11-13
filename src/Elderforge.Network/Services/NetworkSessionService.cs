using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Interfaces.Sessions;
using Serilog;

namespace Elderforge.Network.Services;

public class NetworkSessionService : INetworkSessionService
{
    public int SessionCount => _sessions.Count;

    private readonly ConcurrentDictionary<string, ISessionObject> _sessions = new();
    private readonly ILogger _logger = Log.ForContext<NetworkSessionService>();

    public List<string> GetSessionIds => _sessions.Keys.ToList();

    public ISessionObject? GetSessionObject(string sessionId)
    {
        return _sessions.GetValueOrDefault(sessionId);
    }

    public void AddSession(string sessionId, ISessionObject sessionObject)
    {
        if (_sessions.ContainsKey(sessionId))
        {
            _logger.Warning("Session {sessionId} already exists", sessionId);

            _sessions.TryRemove(sessionId, out _);
        }

        _logger.Debug("Adding session {sessionId}", sessionId);
        _sessions.TryAdd(sessionId, sessionObject);
    }

    public void RemoveSession(string sessionId)
    {
        _logger.Debug("Removing session {sessionId}", sessionId);
        _sessions.TryRemove(sessionId, out _);
    }

    public void UpdateLastActive(string sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var session))
        {
            session.LastActive = DateTime.UtcNow;
        }
    }

    public IEnumerable<ISessionObject> GetExpiredSessions(TimeSpan expirationTime)
    {
        return _sessions
            .Where(x => DateTime.UtcNow - x.Value.LastActive > expirationTime)
            .Select(x => x.Value)
            .ToList();
    }
}
