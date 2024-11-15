using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Events.Network;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Events;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.System;
using Serilog;

namespace Elderforge.Server.Services.System;

public class SessionCheckService : ISessionCheckService, INetworkMessageListener<PongMessage>
{
    private readonly ILogger _logger = Log.ForContext<ISessionCheckService>();

    private readonly INetworkSessionService _networkSessionService;


    private readonly IEventBusService _eventBusService;

    private const int _sessionTimeoutSeconds = 10;

    public SessionCheckService(
        ISchedulerService schedulerService, IEventBusService eventBusService, INetworkServer networkServer,
        INetworkSessionService networkSessionService
    )
    {
        _eventBusService = eventBusService;

        _networkSessionService = networkSessionService;

        networkServer.RegisterMessageListener<PongMessage>(this);

        schedulerService.AddSchedulerJob(
            "check_session_alive",
            TimeSpan.FromSeconds(_sessionTimeoutSeconds),
            OnSessionCheck
        );
    }

    private async Task OnSessionCheck()
    {
        if (_networkSessionService.SessionCount == 0)
        {
            return;
        }

        _logger.Debug("Checking session is alive");
        await _eventBusService.PublishAsync(new SendMessageEvent(string.Empty, new PingMessage()));

        var expiredSessions = _networkSessionService.GetExpiredSessions(TimeSpan.FromSeconds(_sessionTimeoutSeconds));

        foreach (var session in expiredSessions)
        {
            await _eventBusService.PublishAsync(new ClientDisconnectedEvent(session.Peer.Id.ToString()));

            _networkSessionService.RemoveSession(session.Peer.Id.ToString());
        }
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(string sessionId, PongMessage message)
    {
        _logger.Debug("Received pong message from {sessionId}", sessionId);

        _networkSessionService.UpdateLastActive(sessionId);

        return Enumerable.Empty<SessionNetworkMessage>();
    }
}
