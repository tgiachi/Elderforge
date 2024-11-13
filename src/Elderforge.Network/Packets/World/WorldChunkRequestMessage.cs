using Elderforge.Core.Numerics;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Serialization.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Packets.World;


[ProtoContract]
public class WorldChunkRequestMessage : INetworkMessage
{

    [ProtoMember(1)]
    public SerializableVector3Int Position { get; set; }


    public WorldChunkRequestMessage(Vector3Int position)
    {
        Position = new SerializableVector3Int(position);
    }

    public WorldChunkRequestMessage() { }
}
