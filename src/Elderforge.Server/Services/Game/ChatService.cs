using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Attributes.Services;
using Elderforge.Core.Server.Interfaces.Services.Game;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Packets.Chat;
using Serilog;

namespace Elderforge.Server.Services.Game;

[ElderforgeService]
public class ChatService : AbstractGameService, IChatService, INetworkMessageListener<ChatMessage>
{
    private readonly ILogger _logger = Log.Logger.ForContext<ChatService>();

    public ChatService(IEventBusService eventBusService) : base(eventBusService)
    {
    }


    // TODO: implement routing of different types of chat messages
    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(string sessionId, ChatMessage message)
    {
        _logger.Debug("Chat message received from {sessionId}: {message}", sessionId, message.Message);


        return new[]
        {
            new SessionNetworkMessage(message.TargetSessionId, message)
        };
    }
}
