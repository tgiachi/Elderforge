using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Packets.World;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Chunks;
using Serilog;
using Unity.VisualScripting;
using UnityEngine;

public class WorldVisualizer : MonoBehaviour
{
    [Header("Visualization")]
    [SerializeField] private ChunkVisualizer chunkVisualizer;
    [SerializeField] private int renderDistance = 2;
    [SerializeField] private Transform player;


    [Header("Settings")]
    [Range(0.1f, 1.0f)]
    [SerializeField] private float chunkRequestInterval = 0.1f;


    private Vector3Int lastPlayerChunk;
    private HashSet<Vector3Int> requestedChunks;
    private HashSet<Vector3Int> loadedChunks;
    private Queue<WorldChunkRequestMessage> chunkRequestQueue;

    private Queue<Action> _actions = new();


    void Awake()
    {
        ElderforgeInstanceHolder.Initialize(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.DebugLog());

        Log.Information("Initializing Elderforge");

        ElderforgeInstanceHolder.NetworkClient.SubscribeToMessage<WorldChunkResponseMessage>()
            .Subscribe(OnWorldChunkResponse);

        requestedChunks = new HashSet<Vector3Int>();
        loadedChunks = new HashSet<Vector3Int>();
        chunkRequestQueue = new Queue<WorldChunkRequestMessage>();

        ElderforgeInstanceHolder.NetworkClient.Connect("127.0.0.1", 5000);

        StartOutputQueue();
    }

    private void OnWorldChunkResponse(WorldChunkResponseMessage obj)
    {
        Log.Logger.Information("Received chunk for position {Pos}", obj.Position);
        ProcessChunkData(obj);
    }

    void Update()
    {
        if (ElderforgeInstanceHolder.NetworkClient == null)
            return;

        if (!ElderforgeInstanceHolder.NetworkClient.IsConnected)
            return;

        UpdatePlayerChunkPosition();

        while (_actions.Count > 0)
        {
            _actions.Dequeue().Invoke();
        }
    }

    private void StartOutputQueue()
    {
        Task.Run(
            () =>
            {
                while (true)
                {
                    if (chunkRequestQueue.Count > 0)
                    {
                        var request = chunkRequestQueue.Dequeue();
                        ElderforgeInstanceHolder.NetworkClient.SendMessage(request);
                    }

                    ElderforgeInstanceHolder.NetworkClient.PoolEvents();

                    Task.Delay(TimeSpan.FromSeconds(chunkRequestInterval)).Wait();
                }

            }
        );
    }


    private void ProcessChunkData(WorldChunkResponseMessage protoChunk)
    {
        try
        {
            var chunkPos = new Vector3Int(
                protoChunk.Position.X,
                protoChunk.Position.Y,
                protoChunk.Position.Z
            );


            var chunk = protoChunk.Chunk.ToChunkEntity();

            _actions.Enqueue(() =>
            {
                chunkVisualizer.VisualizeChunk(chunk);
            });



            requestedChunks.Remove(chunkPos);
            loadedChunks.Add(chunkPos);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to process chunk data: {e.Message}");
        }
    }

    private void UpdatePlayerChunkPosition()
    {
        var playerPosition = player.position;
        var currentChunk = new Vector3Int(
            Mathf.FloorToInt(playerPosition.x / ChunkEntity.CHUNK_SIZE),
            Mathf.FloorToInt(playerPosition.y / ChunkEntity.CHUNK_SIZE),
            Mathf.FloorToInt(playerPosition.z / ChunkEntity.CHUNK_SIZE)
        );

        if (currentChunk != lastPlayerChunk)
        {
            RequestVisibleChunks(currentChunk);
            UnloadDistantChunks(currentChunk);
            lastPlayerChunk = currentChunk;
        }
    }

    private void RequestVisibleChunks(Vector3Int centerChunk)
    {
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                for (int z = -renderDistance; z <= renderDistance; z++)
                {
                    var chunkPos = centerChunk + new Vector3Int(x, y, z);
                    if (!loadedChunks.Contains(chunkPos) && !requestedChunks.Contains(chunkPos))
                    {
                        QueueChunkRequest(chunkPos);
                    }
                }
            }
        }
    }

    private void QueueChunkRequest(Vector3Int chunkPos)
    {
        var request = new WorldChunkRequestMessage
        {
            Position = new SerializableVector3Int()
            {
                X = chunkPos.x,
                Y = chunkPos.y,
                Z = chunkPos.z
            }
        };

        chunkRequestQueue.Enqueue(request);
        requestedChunks.Add(chunkPos);
    }

    private void UnloadDistantChunks(Vector3Int centerChunk)
    {
        var chunksToUnload = new List<Vector3Int>();
        foreach (var loadedChunk in loadedChunks)
        {
            if (IsChunkTooFar(centerChunk, loadedChunk))
            {
                chunksToUnload.Add(loadedChunk);
            }
        }

        foreach (var chunk in chunksToUnload)
        {
            UnloadChunk(chunk);
        }
    }

    private bool IsChunkTooFar(Vector3Int centerChunk, Vector3Int checkChunk)
    {
        var distance = Vector3Int.Distance(centerChunk, checkChunk);
        return distance > renderDistance + 1; // +1 per un po' di buffer
    }

    private void UnloadChunk(Vector3Int chunkPos)
    {
        // chunkVisualizer.ClearChunk(chunkPos);
        loadedChunks.Remove(chunkPos);
    }

}
