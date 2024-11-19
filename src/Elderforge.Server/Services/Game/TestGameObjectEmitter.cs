using System.Collections.Concurrent;
using System.Numerics;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Events.Engine;
using Elderforge.Core.Server.Events.Network;
using Elderforge.Core.Server.GameObjects.Base;
using Elderforge.Core.Server.Interfaces.Services.Game.Base;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.GameObjects;
using Elderforge.Server.Interfaces;
using Serilog;

namespace Elderforge.Server.Services.Game;

public class TestGameObjectEmitter : AbstractGameService, ITestGameObjectEmitter
{
    private readonly ConcurrentDictionary<string, AbstractGameObject> _gameObjects = new();


    private readonly IGameObjectManagerService _gameObjectManagerService;


    public TestGameObjectEmitter(
        IEventBusService eventBusService, INetworkServer networkServer, ISchedulerService schedulerService,
        IGameObjectManagerService gameObjectManagerService
    ) : base(eventBusService)
    {
        _gameObjectManagerService = gameObjectManagerService;
        SubscribeEvent<ClientConnectedEvent>(OnClientConnected);

        SubscribeEvent<EngineStartedEvent>(
            @event =>
            {
                // schedulerService.AddSchedulerJob(
                //     "generate_game_objects",
                //     TimeSpan.FromSeconds(5),
                //     GenerateRandomGameObjects
                // );
                //
                // schedulerService.AddSchedulerJob(
                //     "move_game_objects",
                //     TimeSpan.FromMilliseconds(1000),
                //     MoveRandomGameObjects
                // );
            }
        );
    }

    private Task GenerateRandomGameObjects()
    {
        if (_gameObjects.Count >= 10)
        {
            return Task.CompletedTask;
        }

        var randomId = Guid.NewGuid().ToString();

        Log.Information("Generating random game object with id {id}", randomId);

        var gameObject = new AbstractGameObject()
        {
            Id = randomId,
            Name = "Test Object " + randomId,
            Position = new Vector3(0, 0, 0),
            Scale = new Vector3(Random.Shared.Next(1, 10), Random.Shared.Next(1, 10), Random.Shared.Next(1, 10)),
            Rotation = new Vector3(0, 0, 0)
        };

        _gameObjects.TryAdd(randomId, gameObject);


        _gameObjectManagerService.AddGameObject(gameObject);

        return Task.CompletedTask;
    }


    private Task MoveRandomGameObjects()
    {
        // Log.Information("Moving random game objects");
        foreach (var gameObject in _gameObjects.Values)
        {
            gameObject.Position = new Vector3(
                gameObject.Position.X + (float)(new Random().NextDouble() * 5),
                gameObject.Position.Y + (float)(new Random().NextDouble() * 5),
                gameObject.Position.Z + (float)(new Random().NextDouble() * 5)
            );

            gameObject.Rotation = new Vector3(
                gameObject.Rotation.X + (float)(new Random().NextDouble() * 4),
                gameObject.Rotation.Y + (float)(new Random().NextDouble() * 4),
                gameObject.Rotation.Z + (float)(new Random().NextDouble() * 4)
            );
        }

        return Task.CompletedTask;
    }

    private void OnClientConnected(ClientConnectedEvent obj)
    {
        foreach (var gameObject in _gameObjects.Values)
        {
            SendNetworkMessage(obj.SessionId, new GameObjectCreateMessage(gameObject));
        }
    }
}
