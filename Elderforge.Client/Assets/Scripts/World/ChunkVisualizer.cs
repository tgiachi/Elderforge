using Elderforge.Shared.Blocks;
using Elderforge.Shared.Chunks;
using Elderforge.Shared.Types;
using System.Collections.Generic;
using UnityEngine;

public class ChunkVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject rockPrefab;
    private Dictionary<Vector3Int, GameObject> blockObjects = new Dictionary<Vector3Int, GameObject>();

    public void VisualizeChunk(ChunkEntity chunk)
    {
        ClearExistingChunk(new Vector3Int(chunk.Position.X, chunk.Position.Y, chunk.Position.Z));

        for (int x = 0; x < ChunkEntity.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < ChunkEntity.CHUNK_SIZE; y++)
            {
                for (int z = 0; z < ChunkEntity.CHUNK_SIZE; z++)
                {
                    // 3. Otteniamo il blocco in questa posizione
                    var block = chunk.GetBlock(x, y, z);

                    // 4. Se non è aria, creiamo un blocco visivo
                    if (block != null && block.Type != BlockType.Air)
                    {
                        CreateVisualBlock(block);
                    }
                }
            }
        }
    }

    private void CreateVisualBlock(BlockEntity block)
    {

        GameObject prefabToUse = GetPrefabForBlockType(block.Type);


        Vector3 worldPosition = new Vector3(
            block.Position.X + 0.5f,
            block.Position.Y + 0.5f,
            block.Position.Z + 0.5f
        );


        GameObject blockObject = Instantiate(
            prefabToUse,
            worldPosition,
            Quaternion.identity,
            transform
        );


        blockObjects[new Vector3Int(block.Position.X, block.Position.Y, block.Position.Z)] = blockObject;


        var blockBehavior = blockObject.AddComponent<BlockBehavior>();
        blockBehavior.Initialize(block.Type, new Vector3Int(block.Position.X, block.Position.Y, block.Position.Z));
    }

    private GameObject GetPrefabForBlockType(BlockType type)
    {
        return type switch
        {
            BlockType.Stone => stonePrefab,
            BlockType.Rock => rockPrefab,
            _ => stonePrefab // fallback default
        };
    }

    private void ClearExistingChunk(Vector3Int chunkPosition)
    {

        Vector3Int startPos = chunkPosition * ChunkEntity.CHUNK_SIZE;
        Vector3Int endPos = startPos + new Vector3Int(
            ChunkEntity.CHUNK_SIZE,
            ChunkEntity.CHUNK_SIZE,
            ChunkEntity.CHUNK_SIZE
        );


        for (int x = startPos.x; x < endPos.x; x++)
        {
            for (int y = startPos.y; y < endPos.y; y++)
            {
                for (int z = startPos.z; z < endPos.z; z++)
                {
                    var pos = new Vector3Int(x, y, z);
                    if (blockObjects.TryGetValue(pos, out GameObject existingBlock))
                    {
                        Destroy(existingBlock);
                        blockObjects.Remove(pos);
                    }
                }
            }
        }
    }


    private class BlockBehavior : MonoBehaviour
    {
        private BlockType type;
        private Vector3Int position;

        public void Initialize(BlockType type, Vector3Int position)
        {
            this.type = type;
            this.position = position;
        }

    }
}
