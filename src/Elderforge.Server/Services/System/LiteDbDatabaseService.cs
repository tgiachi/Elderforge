using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Core.Server.Interfaces.Entities;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Utils;
using Elderforge.Server.Data;
using LiteDB;
using LiteDB.Async;
using Serilog;

namespace Elderforge.Server.Services.System;

public class LiteDbDatabaseService : IDatabaseService
{
    private readonly ILogger _logger = Log.Logger.ForContext<LiteDbDatabaseService>();

    private readonly LiteDatabaseAsync _database;

    private readonly List<DbEntityTypeData> _dbEntityTypes;

    public LiteDbDatabaseService(
        DirectoriesConfig directoriesConfig, ElderforgeServerOptions options, List<DbEntityTypeData> dbEntityTypes
    )
    {
        _dbEntityTypes = dbEntityTypes;
        _database = new LiteDatabaseAsync(
            new ConnectionString()
            {
                Filename = Path.Combine(directoriesConfig[DirectoryType.Database], options.DatabaseFileName),
                Connection = ConnectionType.Shared,
            }
        );

        _dbEntityTypes.ForEach(
            e =>
            {
                var collectionName = GetCollectionName(e.EntityType);
                _database.GetCollection(collectionName, BsonAutoId.Guid);
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
        var startTime = Stopwatch.GetTimestamp();
        var collection = _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));

        entities.ForEach(
            e =>
            {
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
            }
        );


        await collection.InsertAsync(entities);

        var endTime = Stopwatch.GetTimestamp();


        _logger.Debug(
            "Inserted {Count} entities of type {Type} in {Time} ms",
            entities.Count,
            typeof(TEntity).Name,
            StopwatchUtils.GetElapsedMilliseconds(startTime, endTime)
        );
        return entities;
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : IBaseDbEntity
    {
        var startTime = Stopwatch.GetTimestamp();
        var count = await _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).CountAsync();

        var endTime = Stopwatch.GetTimestamp();

        _logger.Debug(
            "Counted {Count} entities of type {Type} in {Time} ms",
            count,
            typeof(TEntity).Name,
            StopwatchUtils.GetElapsedMilliseconds(startTime, endTime)
        );

        return count;
    }

    public Task<TEntity> FindByIdAsync<TEntity>(Guid id) where TEntity : IBaseDbEntity
    {
        return _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).FindByIdAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>() where TEntity : IBaseDbEntity
    {
        var startTime = Stopwatch.GetTimestamp();
        var entities = (await _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity))).FindAllAsync()).ToList();

        var endTime = Stopwatch.GetTimestamp();

        _logger.Debug(
            "Found {Count} entities of type {Type} in {Time} ms",
            entities.Count(),
            typeof(TEntity).Name,
            StopwatchUtils.GetElapsedMilliseconds(startTime, endTime)
        );

        return entities;
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
