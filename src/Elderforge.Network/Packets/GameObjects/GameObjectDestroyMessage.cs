using Elderforge.Network.Interfaces.Messages;
using ProtoBuf;

namespace Elderforge.Network.Packets.GameObjects;


[ProtoContract]
public class GameObjectDestroyMessage : INetworkMessage
{
    [ProtoMember(1)] public string Id { get; set; }

    public GameObjectDestroyMessage(string id)
    {
        Id = id;
    }

    public GameObjectDestroyMessage()
    {
    }

}
