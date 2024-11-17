using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Lights;
using Elderforge.Shared.Interfaces;
using ProtoBuf;

namespace Elderforge.Network.Packets.GameObjects.Lights;

[ProtoContract]
public class LightGameObjectResponseMessage : INetworkMessage
{
    [ProtoMember(1)] public SerializableLightEntity Light { get; set; }

    public LightGameObjectResponseMessage()
    {
    }

    public LightGameObjectResponseMessage(ILightGameObject light)
    {
        Light = new SerializableLightEntity(light);
    }
}
