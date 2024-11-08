using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Events;
using Elderforge.Core.Server.Events.Network;
using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.Chat;
using Serilog;

namespace Elderforge.Server.Services;

public class ChatService : IChatService, INetworkMessageListener<ChatMessage>
{
    private readonly ILogger _logger = Log.Logger.ForContext<ChatService>();

    private readonly INetworkServer _networkServer;
    private readonly IEventBusService _eventBusService;

    private readonly IScriptEngineService _scriptEngineService;

    public ChatService(
        INetworkServer networkServer, IEventBusService eventBusService, IScriptEngineService scriptEngineService
    )
    {
        _networkServer = networkServer;
        _eventBusService = eventBusService;
        _scriptEngineService = scriptEngineService;

        _eventBusService.Subscribe<ClientConnectedEvent>(OnClientConnected);
        _networkServer.RegisterMessageListener(this);
    }

    private void OnClientConnected(ClientConnectedEvent obj)
    {
    }


    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(string sessionId, ChatMessage message)
    {
        return [];
    }
}
