using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.GameObjects;
using Elderforge.Network.Packets.GameObjects.Lights;
using Elderforge.Network.Serialization.Lights;
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

    private readonly SemaphoreSlim _gameObjectsLock = new(1);


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
            foreach (var item in e.NewItems)
            {
                if (item is ILightGameObject lightGameObject)
                {
                    LightAdded(lightGameObject);
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems)
            {
                if (item is ILightGameObject lightGameObject)
                {
                    LightRemoved(lightGameObject);
                }
            }
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


    private void LightAdded(ILightGameObject lightGameObject)
    {
        _logger.Debug("Light added {id} in position: {Pos}", lightGameObject.Id, lightGameObject.Position);

        lightGameObject.PositionSubject.Subscribe(_ => SendLightUpdate(lightGameObject));
        lightGameObject.ScaleSubject.Subscribe(_ => SendLightUpdate(lightGameObject));
        lightGameObject.LightColorSubject.Subscribe(_ => SendLightUpdate(lightGameObject));
        lightGameObject.LightIntensitySubject.Subscribe(_ => SendLightUpdate(lightGameObject));

        _ = SendLightUpdate(lightGameObject);
    }

    private void LightRemoved(ILightGameObject lightGameObject)
    {
        _logger.Debug("Light removed {id}", lightGameObject.Id);

        _ = SendDestroyObject(lightGameObject);
    }


    private async Task SendLightUpdate(ILightGameObject lightGameObject)
    {
        foreach (var session in _networkSessionService.GetSessionObjectCanSee(_renderDistance, lightGameObject.Position))
        {
            await _networkServer.SendMessageAsync(
                new SessionNetworkMessage(session.Id, CreateLightGameObjectResponseMessage(lightGameObject))
            );
        }
    }

    private async Task SendDestroyObject(IGameObject gameObject)
    {
        foreach (var session in _networkSessionService.GetSessionObjectCanSee(_renderDistance, gameObject.Position))
        {
            await _networkServer.SendMessageAsync(
                new SessionNetworkMessage(session.Id, new GameObjectDestroyMessage(gameObject.Id))
            );
        }
    }

    private static LightGoUpdateResponseMessage CreateLightGameObjectResponseMessage(ILightGameObject lightGameObject)
    {
        return new LightGoUpdateResponseMessage
        {
            Light = new SerializableLightEntity(lightGameObject)
        };
    }

    public void Dispose()
    {
        _gameObjectsLock.Dispose();
        _gameObjects.CollectionChanged -= OnCollectionChanged;
    }
}
