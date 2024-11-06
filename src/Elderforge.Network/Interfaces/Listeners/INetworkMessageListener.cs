using System.Collections.Generic;
using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Interfaces.Listeners;

public interface INetworkMessageListener<in TMessage> where TMessage : class, INetworkMessage
{
    ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(string sessionId, TMessage message);
}
