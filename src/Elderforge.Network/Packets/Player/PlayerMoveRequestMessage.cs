using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Packets.Player;

[ProtoContract]
public class PlayerMoveRequestMessage : INetworkMessage
{
    [ProtoMember(1)] public SerializableVector3 Position { get; set; }
    [ProtoMember(2)] public SerializableVector3 Rotation { get; set; }

    public PlayerMoveRequestMessage(string id, SerializableVector3 position, SerializableVector3 rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public PlayerMoveRequestMessage()
    {
    }

    public override string ToString()
    {
        return $"PlayerMoveRequestMessage Position: {Position}, Rotation: {Rotation}";
    }
}
