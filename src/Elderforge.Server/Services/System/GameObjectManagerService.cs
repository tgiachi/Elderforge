using System.Collections.Concurrent;
using System.Numerics;
using Elderforge.Core.Extensions;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Events.Network;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Interfaces.Sessions;
using Elderforge.Network.Packets.GameObjects;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Server.Extensions;
using Elderforge.Shared.Interfaces;

namespace Elderforge.Server.Services.System;

public class GameObjectManagerService : IGameObjectManagerService
{
    private readonly BlockingCollection<IGameObject> _gameObjects = new();

    private readonly INetworkSessionService _networkSessionService;

    private readonly INetworkServer _networkServer;

    private float _renderDistance = 100;


    public GameObjectManagerService(INetworkSessionService networkSessionService, INetworkServer networkServer)
    {
        _networkSessionService = networkSessionService;
        _networkServer = networkServer;
    }


    public void AddGameObject<TEntity>(TEntity entity) where TEntity : class, IGameObject
    {
        _gameObjects.Add(entity);

        entity.ScaleChanged += ScaleChanged;
        entity.PositionChanged += PositionChanged;
        entity.RotationChanged += RotationChanged;
    }


    public void RemoveGameObject<TEntity>(TEntity entity) where TEntity : class, IGameObject
    {
        _gameObjects.TryTake(out var go);

        go.ScaleChanged -= ScaleChanged;
        go.PositionChanged -= PositionChanged;
        go.RotationChanged -= RotationChanged;
    }

    public void RemoveGameObject(string id)
    {
        var go = _gameObjects.FirstOrDefault(x => x.Id == id);

        if (go != null)
        {
            RemoveGameObject(go);
        }
    }

    private void ScaleChanged(IGameObject gameObject, Vector3 scale)
    {
    }

    private void PositionChanged(IGameObject gameObject, Vector3 newValue)
    {
        var message = new GameObjectMoveMessage(gameObject);

        message.Position = new SerializableVector3(newValue);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        }
    }

    private void RotationChanged(IGameObject gameObject, Vector3 newValue)
    {
        var message = new GameObjectMoveMessage(gameObject);

        message.Rotation = new SerializableVector3(newValue);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        }
    }
}
