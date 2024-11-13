using System.Collections.Generic;
using Elderforge.Network.Serialization.Chunks;
using Elderforge.Network.Serialization.Numerics;
using ProtoBuf;

namespace Elderforge.Network.Serialization.World;

[ProtoContract]
public class SerializableWorld
{
    [ProtoMember(1)] public List<SerializableChunkEntity> Chunks { get; set; }

    [ProtoMember(2)] public int WorldSeed { get; set; }

    [ProtoMember(3)] public SerializableVector3Int WorldSize { get; set; }


    public SerializableWorld()
    {
        Chunks = new List<SerializableChunkEntity>();
    }
}
