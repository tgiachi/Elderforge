using Elderforge.Core.Interfaces.Events;
using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Events;

public class SendMessageEvent : IElderforgeEvent
{

    public string SessionId { get; set; }

    public INetworkMessage Message { get; set; }

}
