using System;
using System.Collections.Concurrent;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Packets.World;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Shared.Chunks;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.World;
using Elderforge.Network.Packets.GameObjects;
using UnityEngine;

public class WorldVisualizer : MonoBehaviour
{
    [Header("Visualization")]
    [SerializeField] private ChunkVisualizer chunkVisualizer;
    [SerializeField] private int renderDistance = 1;
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private int numOfRequestThreads = 1;

    [SerializeField] private bool isRenderEnabled;


    [SerializeField] private GameObjectManager gameObjectManager;



    private Vector3Int lastPlayerChunk = new Vector3Int(-1, 0, -1);
    private HashSet<Vector3Int> requestedChunks;
    private HashSet<Vector3Int> loadedChunks;
    private ConcurrentQueue<WorldChunkRequestMessage> chunkRequestQueue;

    private Queue<Action> _actions = new();

    void Start()
    {
        ElderforgeInstanceHolder.Initialize(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.DebugLog());

        Log.Information("Initializing Elderforge");

        ElderforgeInstanceHolder.NetworkClient.SubscribeToMessage<WorldChunkResponseMessage>()
            .Subscribe(OnWorldChunkResponse);


        ElderforgeInstanceHolder.NetworkClient
            .SubscribeToMessage<GameObjectCreateMessage>()

            .Subscribe(
                (m) =>
                {
                    _actions.Enqueue(() => { gameObjectManager.OnGameObjectCreated(m); });
                });

        ElderforgeInstanceHolder
            .NetworkClient
            .SubscribeToMessage<GameObjectDestroyMessage>()
            .Subscribe(
                (m) =>
                {
                    _actions.Enqueue(() => { gameObjectManager.OnGameObjectDestroyed(m); });
                });

        ElderforgeInstanceHolder
            .NetworkClient
            .SubscribeToMessage<GameObjectMoveMessage>()

            .Subscribe(
                (m) =>
                {
                    _actions.Enqueue(() => { gameObjectManager.OnGameObjectMoved(m); });
                });

        requestedChunks = new HashSet<Vector3Int>();
        loadedChunks = new HashSet<Vector3Int>();
        chunkRequestQueue = new ConcurrentQueue<WorldChunkRequestMessage>();

        ElderforgeInstanceHolder.NetworkClient.Connect("127.0.0.1", 5000);

        StartOutputQueue();
        StartOutputQueueThreads();

    }

    private void OnWorldChunkResponse(WorldChunkResponseMessage obj)
    {
        Log.Logger.Information("Received chunk for position {Pos}", obj.Position);
        ProcessChunkData(obj);
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


            if (isRenderEnabled)
            {
                _actions.Enqueue(() => { chunkVisualizer.VisualizeChunk(chunk); });
            }



            requestedChunks.Remove(chunkPos);
            loadedChunks.Add(chunkPos);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to process chunk data: {e.Message}");
        }
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
            async () =>
            {
                while (true)
                {
                    ElderforgeInstanceHolder.NetworkClient.PoolEvents();
                }

            }
        );
    }

    private void StartOutputQueueThreads()
    {
        foreach (var _ in Enumerable.Range(1, numOfRequestThreads))
        {
            Task.Run(
                async () =>
                {
                    while (true)
                    {
                        if (chunkRequestQueue.Count > 0)
                        {
                            chunkRequestQueue.TryDequeue(out var request);
                            await ElderforgeInstanceHolder.NetworkClient.SendMessageAsync(request);
                        }
                    }
                }
            );
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

                    if (IsChunkVisible(chunkPos) && !loadedChunks.Contains(chunkPos) && !requestedChunks.Contains(chunkPos))
                    {
                        QueueChunkRequest(chunkPos);
                    }
                }
            }
        }
    }

    private bool IsChunkVisible(Vector3Int chunkPos)
    {
        Vector3 chunkWorldPosition = new Vector3(
            chunkPos.x * ChunkEntity.CHUNK_SIZE,
            chunkPos.y * ChunkEntity.CHUNK_SIZE,
            chunkPos.z * ChunkEntity.CHUNK_SIZE
        );

        Bounds chunkBounds = new Bounds(chunkWorldPosition, Vector3.one * ChunkEntity.CHUNK_SIZE);

        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(playerCamera), chunkBounds);
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
            if (!IsChunkVisible(loadedChunk) || IsChunkTooFar(centerChunk, loadedChunk))
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
        return distance > renderDistance + 1;
    }

    private void UnloadChunk(Vector3Int chunkPos)
    {
        chunkVisualizer.ClearChunk(chunkPos);
        loadedChunks.Remove(chunkPos);
    }
}
