using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Packets.Player;

[ProtoContract]
public class PlayerUpdateResponseMessage : INetworkMessage
{
    [ProtoMember(1)] public string Id { get; set; }
    [ProtoMember(2)] public SerializableVector3 Position { get; set; }
    [ProtoMember(3)] public SerializableVector3 Rotation { get; set; }

    public PlayerUpdateResponseMessage(string id, SerializableVector3 position, SerializableVector3 rotation)
    {
        Id = id;
        Position = position;
        Rotation = rotation;
    }

    public PlayerUpdateResponseMessage()
    {
    }

    public override string ToString()
    {
        return $"Id: {Id}, Position: {Position}, Rotation: {Rotation}";
    }
}
