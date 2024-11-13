using System.IO.Compression;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Serialization.Chunks;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Network.Serialization.World;
using Elderforge.Shared.Chunks;
using ProtoBuf;

namespace Elderforge.Server.Services.System;

public class WorldSerializer
{
    public static void SaveWorld(string filePath, IWorldGeneratorService worldGen, IProgress<float> progress = null)
    {
        var serializableWorld = new SerializableWorld
        {
            WorldSeed = worldGen.WorldSeed,
            WorldSize = new SerializableVector3Int(worldGen.GetWorldSize()),
            Chunks = new List<SerializableChunkEntity>()
        };

        var allChunkPositions = worldGen.GetAllChunkPositions().ToList();
        int totalChunks = allChunkPositions.Count;
        int currentChunk = 0;

        foreach (var chunkPos in allChunkPositions)
        {
            if (worldGen.TryGetChunk(chunkPos, out ChunkEntity chunk))
            {
                serializableWorld.Chunks.Add(new SerializableChunkEntity(chunk));
            }

            currentChunk++;
            progress?.Report((float)currentChunk / totalChunks);
        }

        using var file = File.Create(filePath);
        Serializer.Serialize(file, serializableWorld);
    }

    public static List<ChunkEntity> LoadWorld(string filePath, IProgress<float> progress = null)
    {
        SerializableWorld serializableWorld;
        using (var file = File.OpenRead(filePath))
        {
            serializableWorld = Serializer.Deserialize<SerializableWorld>(file);
        }


        int totalChunks = serializableWorld.Chunks.Count;

        var chunks = new List<ChunkEntity>();
        for (int i = 0; i < totalChunks; i++)
        {
            var serializableChunk = serializableWorld.Chunks[i];
            var chunk = serializableChunk.ToChunkEntity();

            chunks.Add(chunk);

            progress?.Report((float)i / totalChunks);
        }

        return chunks;
    }


    public static void SaveCompressedWorld(
        string filePath, IWorldGeneratorService worldGen, IProgress<float> progress = null
    )
    {
        using var file = File.Create(filePath);
        using var gzip = new GZipStream(file, CompressionLevel.Optimal);
        var serializableWorld = new SerializableWorld
        {
            WorldSeed = worldGen.WorldSeed,
            WorldSize = new SerializableVector3Int(worldGen.GetWorldSize()),
            Chunks = worldGen.GetLoadedChunks()
                .Select(chunk => new SerializableChunkEntity(chunk))
                .ToList()
        };

        Serializer.Serialize(gzip, serializableWorld);
    }

    public static List<ChunkEntity> LoadCompressedWorld(string filePath, IProgress<float> progress = null)
    {
        using var file = File.OpenRead(filePath);
        using var gzip = new GZipStream(file, CompressionMode.Decompress);
        var serializableWorld = Serializer.Deserialize<SerializableWorld>(gzip);

        var dimensions = new WorldDimensions(
            serializableWorld.WorldSize.X,
            serializableWorld.WorldSize.Y,
            serializableWorld.WorldSize.Z
        );

        var chunks = new List<ChunkEntity>();

        int totalChunks = serializableWorld.Chunks.Count;

        for (int i = 0; i < totalChunks; i++)
        {
            var serializableChunk = serializableWorld.Chunks[i];
            var chunk = serializableChunk.ToChunkEntity();
            chunks.Add(chunk);

            progress?.Report((float)i / totalChunks);
        }

        return chunks;
    }
}
