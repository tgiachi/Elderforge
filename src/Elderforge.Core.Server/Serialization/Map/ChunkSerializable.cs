using ProtoBuf;

namespace Elderforge.Core.Server.Serialization.Map;

[ProtoContract]
public class ChunkSerializable
{
    [ProtoMember(1)] public int X { get; }


    [ProtoMember(2)] public int Z { get; }

    [ProtoMember(3)] public BlockSerializable[,,] Blocks { get; }

    [ProtoMember(4)] public int ChunkSize { get; }

    [ProtoMember(5)] public int ChunkHeight { get; }

    public ChunkSerializable(int x, int z, int chunkSize, int chunkHeight)
    {
        X = x;
        Z = z;
        ChunkSize = chunkSize;
        ChunkHeight = chunkHeight;
        Blocks = new BlockSerializable[chunkSize, chunkHeight, chunkSize];
    }

    public void SetBlock(int x, int y, int z, BlockSerializable block)
    {
        if (x >= 0 && x < ChunkSize && y >= 0 && y < ChunkHeight && z >= 0 && z < ChunkSize)
        {
            Blocks[x, y, z] = block;
        }
    }

    public BlockSerializable GetBlock(int x, int y, int z)
    {
        if (x >= 0 && x < ChunkSize && y >= 0 && y < ChunkHeight && z >= 0 && z < ChunkSize)
        {
            return Blocks[x, y, z];
        }

        return null;
    }
}
