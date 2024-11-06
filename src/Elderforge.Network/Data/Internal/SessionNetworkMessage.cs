using Elderforge.Network.Interfaces.Messages;

namespace Elderforge.Network.Data.Internal;

public class SessionNetworkMessage
{
    public string SessionId { get; set; }

    public INetworkMessage Packet { get; set; }

    public SessionNetworkMessage(string sessionId, INetworkMessage packet)
    {
        SessionId = sessionId;
        Packet = packet;
    }

    public SessionNetworkMessage()
    {

    }

    public override string ToString() => $"SessionId: {SessionId}, Packet: {Packet.GetType()}";
}