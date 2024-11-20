using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.Player;

[ProtoContract]
public class PlayerDisconnectedMessage : INetworkMessage
{
    [ProtoMember(1)] public string Id { get; set; }


    public PlayerDisconnectedMessage()
    {
    }

    public PlayerDisconnectedMessage(string id)
    {
        Id = id;
    }
}
