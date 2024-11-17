using System.Collections.Concurrent;
using System.Numerics;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Shared.Interfaces;

namespace Elderforge.Server.Services.System;

public class GameObjectManagerService : IGameObjectManagerService
{
    private readonly BlockingCollection<IGameObject> _gameObjects = new();

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
    }

    private void RotationChanged(IGameObject gameObject, Vector3 newValue)
    {
    }
}
