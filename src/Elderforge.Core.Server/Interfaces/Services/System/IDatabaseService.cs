using System.Linq.Expressions;
using Elderforge.Core.Server.Interfaces.Entities;

namespace Elderforge.Core.Server.Interfaces.Services.System;

public interface IDatabaseService : IDisposable
{
    Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : IBaseDbEntity;

    Task<List<TEntity>> InsertAsync<TEntity>(List<TEntity> entities) where TEntity : IBaseDbEntity;

    Task<int> CountAsync<TEntity>() where TEntity : IBaseDbEntity;

    Task<TEntity> FindByIdAsync<TEntity>(Guid id) where TEntity : IBaseDbEntity;

    Task<IEnumerable<TEntity>> FindAllAsync<TEntity>() where TEntity : IBaseDbEntity;

    Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : IBaseDbEntity;

    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : IBaseDbEntity;

    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : IBaseDbEntity;

    Task DeleteAsync<TEntity>(Guid id) where TEntity : IBaseDbEntity;

    Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : IBaseDbEntity;

    Task DeleteAllAsync<TEntity>() where TEntity : IBaseDbEntity;

    Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : IBaseDbEntity;
}
