using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Interfaces;
using ProtoBuf;

namespace Elderforge.Network.Packets.GameObjects;

[ProtoContract]
public class GameObjectCreateMessage : INetworkMessage
{
    [ProtoMember(1)] public string Id { get; set; }

    [ProtoMember(2)] public string Name { get; set; }

    [ProtoMember(3)] public SerializableVector3 Position { get; set; }

    [ProtoMember(4)] public SerializableVector3 Scale { get; set; }

    [ProtoMember(5)] public SerializableVector3 Rotation { get; set; }

    public GameObjectCreateMessage(
        string id, string name, SerializableVector3 position, SerializableVector3 scale,
        SerializableVector3 rotation
    )
    {
        Id = id;
        Name = name;
        Position = position;
        Scale = scale;
        Rotation = rotation;
    }

    public GameObjectCreateMessage(IGameObject gameObject)
    {
        Id = gameObject.Id;
        Name = gameObject.Name;
        Position = new SerializableVector3(gameObject.Position);
        Scale = new SerializableVector3(gameObject.Scale);
        Rotation = new SerializableVector3(gameObject.Rotation);
    }

    public GameObjectCreateMessage()
    {
    }
}
