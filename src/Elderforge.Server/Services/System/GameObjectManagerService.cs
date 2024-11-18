using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Numerics;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Events.GameObjects;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.GameObjects;
using Elderforge.Network.Serialization.Numerics;
using Elderforge.Server.Extensions;
using Elderforge.Shared.Interfaces;
using Serilog;

namespace Elderforge.Server.Services.System;

public class GameObjectManagerService : IGameObjectManagerService
{
    private readonly IEventBusService _eventBusService;

    private readonly ILogger _logger = Log.Logger.ForContext<GameObjectManagerService>();

    private readonly ObservableCollection<IGameObject> _gameObjects = new();

    private readonly INetworkSessionService _networkSessionService;

    private readonly INetworkServer _networkServer;

    private float _renderDistance = 100;

    private SemaphoreSlim _gameObjectsLock = new(1);


    public GameObjectManagerService(
        INetworkSessionService networkSessionService, INetworkServer networkServer, IEventBusService eventBusService
    )
    {
        _networkSessionService = networkSessionService;
        _networkServer = networkServer;
        _eventBusService = eventBusService;

        _gameObjects.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (IGameObject item in e.NewItems)
            {
                _logger.Verbose("GameObject added: {item}", item);

                item.ScaleChanged += ScaleChanged;
                item.PositionChanged += PositionChanged;
                item.RotationChanged += RotationChanged;

                _eventBusService.PublishAsync(new GameObjectAddedEvent(item.Id));

                GameObjectAdded(item);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (IGameObject item in e.OldItems)
            {
                _logger.Verbose("GameObject removed: {item}", item);

                item.ScaleChanged -= ScaleChanged;
                item.PositionChanged -= PositionChanged;
                item.RotationChanged -= RotationChanged;

                _eventBusService.PublishAsync(new GameObjectRemovedEvent(item.Id));

                GameObjectRemoved(item);
            }
        }
    }


    private void GameObjectAdded(IGameObject gameObject)
    {
        var message = new GameObjectCreateMessage(gameObject);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        }
    }

    private void GameObjectRemoved(IGameObject gameObject)
    {
        var message = new GameObjectDestroyMessage(gameObject.Id);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        }
    }

    public void AddGameObject<TEntity>(TEntity entity) where TEntity : class, IGameObject
    {
        _gameObjectsLock.Wait();
        _gameObjects.Add(entity);
        _gameObjectsLock.Release();
    }

    public void RemoveGameObject<TEntity>(TEntity entity) where TEntity : class, IGameObject
    {
        _gameObjectsLock.Wait();
        _gameObjects.Remove(entity);

        entity.ScaleChanged -= ScaleChanged;
        entity.PositionChanged -= PositionChanged;
        entity.RotationChanged -= RotationChanged;

        _gameObjectsLock.Release();
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
        _logger.Verbose("Scale changed: {gameObject} {scale}", gameObject, scale);
        // var message = new GameObjectMoveMessage(gameObject);
        //
        // message.Scale = new SerializableVector3(scale);
        //
        // foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        // {
        //     _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        // }
    }

    private void PositionChanged(IGameObject gameObject, Vector3 newValue)
    {
        _logger.Verbose("Position changed: {gameObject} {newValue}", gameObject, newValue);
        var message = new GameObjectMoveMessage(gameObject);

        message.Position = new SerializableVector3(newValue);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        }
    }

    private void RotationChanged(IGameObject gameObject, Vector3 newValue)
    {
        _logger.Verbose("Rotation changed: {gameObject} {newValue}", gameObject, newValue);
        var message = new GameObjectMoveMessage(gameObject);

        message.Rotation = new SerializableVector3(newValue);

        foreach (var player in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            _networkServer.SendMessageAsync(new SessionNetworkMessage(player.Id, message));
        }
    }
}
