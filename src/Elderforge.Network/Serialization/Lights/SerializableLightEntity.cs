using Elderforge.Network.Serialization.GameObjects;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Interfaces;
using Elderforge.Shared.Types;
using ProtoBuf;

namespace Elderforge.Network.Serialization.Lights;

[ProtoContract]
public class SerializableLightEntity : SerializableGameObject
{
    [ProtoMember(1)] public LightType LightType { get; set; }

    [ProtoMember(2)] public float LightIntensity { get; set; }

    [ProtoMember(3)] public string LightColor { get; set; }

    public SerializableLightEntity()
    {
    }

    public SerializableLightEntity(ILightGameObject light)
    {
        LightType = light.LightType;
        LightIntensity = light.LightIntensity;
        LightColor = light.LightColor;

        Position = new SerializableVector3(light.Position);
        Scale = new SerializableVector3(light.Scale);
        Rotation = new SerializableVector3(light.Rotation);
    }

    public override string ToString()
    {
        return
            $"SerializableLightEntity: LightType: {LightType}, LightIntensity: {LightIntensity}, LightColor: {LightColor}, Position: {Position}, Scale: {Scale}, Rotation: {Rotation}";
    }
}
