using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Packets.Player;

[ProtoContract]
public class PlayerMoveResponseMessage : INetworkMessage
{
    [ProtoMember(1)] public SerializableVector3 Position { get; set; }

    [ProtoMember(2)] public SerializableVector3 Rotation { get; set; }

    public PlayerMoveResponseMessage(string id, SerializableVector3 position, SerializableVector3 rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public PlayerMoveResponseMessage()
    {
    }

    public override string ToString()
    {
        return $"PlayerMoveResponseMessage";
    }
}
