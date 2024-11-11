using ProtoBuf;

namespace Elderforge.Core.Server.Serialization.Map;


[ProtoContract]
public class MapSerializable
{
    [ProtoMember(1)]
    public int Width { get; set; }

    [ProtoMember(2)]
    public int Height { get; set; }

    [ProtoMember(3)]
    public int Depth { get; set; }

    [ProtoMember(4)]
    public List<ChunkSerializable> Chunks { get; set; } = new();

    public MapSerializable()
    {


    }

}
