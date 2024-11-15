using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Interfaces;
using ProtoBuf;

namespace Elderforge.Network.Packets.GameObjects;

[ProtoContract]
public class GameObjectMoveMessage : INetworkMessage
{
    [ProtoMember(1)] public string Id { get; set; }

    [ProtoMember(2)] public SerializableVector3 Position { get; set; }

    [ProtoMember(3)] public SerializableVector3 Rotation { get; set; }

    public GameObjectMoveMessage(
        string id, SerializableVector3 position, SerializableVector3 rotation
    )
    {
        Id = id;
        Position = position;
        Rotation = rotation;
    }

    public GameObjectMoveMessage(IGameObject gameObject)
    {
        Id = gameObject.Id;
        Position = new SerializableVector3(gameObject.Position);
        Rotation = new SerializableVector3(gameObject.Rotation);
    }

    public GameObjectMoveMessage()
    {
    }

    public override string ToString()
    {
        return $"GameObjectMoveMessage: Id: {Id}, Position: {Position}, Rotation: {Rotation}";
    }
}
