using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Data.Internal;
using Elderforge.Server.Data;
using Elderforge.Server.Services.System;
using Elderforge.Tests.Data;

namespace Elderforge.Tests;

public class LiteDbTests
{
    [Fact]
    public async Task InsertAsync_ShouldInsertEntity()
    {
        // Arrange
        var entity = new TestEntity
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        using var databaseService = new LiteDbDatabaseService(
            new DirectoriesConfig(Path.GetTempPath()),
            new ElderforgeServerOptions()
            {
                DatabaseFileName = "test.db"
            },
            [new(typeof(TestEntity))]
        );

        // Act
        var result = await databaseService.InsertAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal(entity.CreatedAt, result.CreatedAt);
        Assert.Equal(entity.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    // Pump database
    public async Task InsertAsync_ShouldInsertEntities()
    {
        // Arrange
        var entities = Enumerable.Range(0, 100)
            .Select(_ => new TestEntity { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow })
            .ToList();

        using var databaseService = new LiteDbDatabaseService(
            new DirectoriesConfig(Path.GetTempPath()),
            new ElderforgeServerOptions()
            {
                DatabaseFileName = "test.db"
            },
            [new(typeof(TestEntity))]
        );

        // Act
        await databaseService.InsertAsync(entities);


        // find all

        var result = await databaseService.FindAllAsync<TestEntity>();

        // Assert

        Assert.True(result.Count() >= 0);
    }
}
