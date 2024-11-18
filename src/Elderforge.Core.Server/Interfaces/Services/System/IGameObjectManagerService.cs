using Elderforge.Shared.Interfaces;

namespace Elderforge.Core.Server.Interfaces.Services.System;

public interface IGameObjectManagerService
{
    void AddGameObject<TEntity>(TEntity entity) where TEntity : class, IGameObject;

    void RemoveGameObject<TEntity>(TEntity entity) where TEntity : class, IGameObject;

    void RemoveGameObject(string id);
}