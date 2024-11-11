using Elderforge.Core.Server.Data.Directories;
using Elderforge.Server.Services.System;
using Serilog;

namespace Elderforge.Tests;

public class MapGenerationTests
{
    public MapGenerationTests()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    [Fact]
    public async Task TestGenerateMapAsync()
    {
        var mapGenerationService = new MapGenerationService(new DirectoriesConfig(Path.GetTempPath()));

        var chunks = await mapGenerationService.GenerateMapAsync(128, 128);

        Assert.Equal(64, chunks.Count);
    }

    [Fact]
    public async Task TestGenerateMapSerializableAsync()
    {
        var mapGenerationService = new MapGenerationService(new DirectoriesConfig(Path.GetTempPath()));

        var chunks = await mapGenerationService.GenerateMapAsync(128, 128);

        var map = await mapGenerationService.GenerateMapSerializableAsync(128, 128, chunks);

        Assert.Equal(128, map.Width);
        Assert.Equal(128, map.Height);
    }

    // [Fact]
    // public async Task TestGenerateMapAndSave()
    // {
    //     var mapGenerationService = new MapGenerationService(new DirectoriesConfig(Path.GetTempPath()));
    //
    //     var chunks = await mapGenerationService.GenerateMapAsync(128, 128);
    //
    //     var map = await mapGenerationService.GenerateMapSerializableAsync(128, 128, chunks);
    //
    //     var memoryStream = new MemoryStream();
    //     Serializer.Serialize(memoryStream, map);
    //
    //     var bytes = memoryStream.ToArray();
    //
    //     Assert.NotEmpty(bytes);
    // }
}