using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Packets.Chat;
using Serilog;

namespace Elderforge.Server.Services;

public class ChatService : IChatService, INetworkMessageListener<ChatMessage>
{
    private readonly ILogger _logger = Log.Logger.ForContext<ChatService>();



    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(string sessionId, ChatMessage message)
    {


        return [];
    }
}
