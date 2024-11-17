using System.Numerics;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Interfaces;
using ProtoBuf;

namespace Elderforge.Network.Serialization.GameObjects;

[ProtoContract]
public class SerializableGameObject
{
    [ProtoMember(1)] public string Id { get; set; }

    [ProtoMember(2)] public string Name { get; set; }

    [ProtoMember(3)] public SerializableVector3 Position { get; set; }

    [ProtoMember(4)] public SerializableVector3 Rotation { get; set; }

    [ProtoMember(5)] public SerializableVector3 Scale { get; set; }


    public SerializableGameObject()
    {
    }

    public SerializableGameObject(IGameObject gameObject)
    {
        Id = gameObject.Id;
        Name = gameObject.Name;
        Position = new SerializableVector3(gameObject.Position);
        Rotation = new SerializableVector3(gameObject.Rotation);
        Scale = new SerializableVector3(gameObject.Scale);
    }

    public override string ToString()
    {
        return $"SerializableGameObject: Id: {Id}, Name: {Name}, Position: {Position}, Rotation: {Rotation}, Scale: {Scale}";
    }
}
