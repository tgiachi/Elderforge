using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Blocks;
using Elderforge.Shared.Types;
using ProtoBuf;

namespace Elderforge.Network.Serialization.Blocks;

[ProtoContract]
public class SerializableBlockEntity
{
    [ProtoMember(1)]
    public BlockType Type { get; set; }

    [ProtoMember(2)]
    public SerializableVector3Int Position { get; set; }


    public SerializableBlockEntity() { }

    public SerializableBlockEntity(BlockEntity block)
    {
        Type = block.Type;
        Position = new SerializableVector3Int(block.Position);
    }

    public BlockEntity ToBlockEntity()
    {
        return new BlockEntity(Type, Position.ToVector3Int());
    }
}
