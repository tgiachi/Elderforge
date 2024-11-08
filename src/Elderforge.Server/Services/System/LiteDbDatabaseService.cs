using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Interfaces.Entities;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Types;
using Elderforge.Server.Data;
using LiteDB;
using LiteDB.Async;
using Serilog;

namespace Elderforge.Server.Services.System;

public class LiteDbDatabaseService : IDatabaseService
{
    private readonly ILogger _logger = Log.Logger.ForContext<LiteDbDatabaseService>();

    private readonly LiteDatabaseAsync _database;

    public LiteDbDatabaseService(DirectoriesConfig directoriesConfig, ElderforgeServerOptions options)
    {
        _database = new LiteDatabaseAsync(
            new ConnectionString()
            {
                Filename = Path.Combine(directoriesConfig[DirectoryType.Database], options.DatabaseFileName),
                Connection = ConnectionType.Shared,
                InitialSize = 1024
            }
        );
    }


    private static string GetCollectionName(Type type)
    {
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();

        return tableAttribute?.Name ?? type.Name;
    }


    public async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : IBaseDbEntity
    {
        InsertAsync([entity]);
        return entity;
    }

    public async Task<List<TEntity>> InsertAsync<TEntity>(List<TEntity> entities) where TEntity : IBaseDbEntity
    {
        var collection = _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));

        entities.ForEach(
            e =>
            {
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
            }
        );

        await collection.InsertAsync(entities);

        return entities;
    }

    public Task<int> CountAsync<TEntity>() where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).CountAsync();
    }

    public Task<TEntity> FindByIdAsync<TEntity>(Guid id) where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).FindByIdAsync(id);
    }

    public Task<IEnumerable<TEntity>> FindAllAsync<TEntity>() where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).FindAllAsync();
    }

    public Task<IEnumerable<TEntity>> QueryAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).FindAsync(predicate);
    }

    public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : IBaseDbEntity
    {
        entity.UpdatedAt = DateTime.UtcNow;
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).UpdateAsync(entity);
    }

    public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).DeleteAsync(entity.Id);
    }

    public Task DeleteAsync<TEntity>(Guid id) where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).DeleteAsync(id);
    }

    public Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).DeleteManyAsync(predicate);
    }

    public Task DeleteAllAsync<TEntity>() where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).DeleteManyAsync(e => true);
    }

    public Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).ExistsAsync(predicate);
    }

    public void Dispose()
    {
        _database.CommitAsync();
        _database.Dispose();
    }
}
