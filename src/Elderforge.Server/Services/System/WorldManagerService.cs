using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Packets.World;
using Serilog;

namespace Elderforge.Server.Services.System;

public class WorldManagerService : IWorldManagerService, INetworkMessageListener<WorldChunkRequestMessage>
{
    private readonly ILogger _logger = Log.ForContext<WorldManagerService>();

    private readonly IWorldGeneratorService _worldGeneratorService;

    public WorldManagerService(IWorldGeneratorService worldGeneratorService, INetworkServer networkServer)
    {
        _worldGeneratorService = worldGeneratorService;

        networkServer.RegisterMessageListener(this);
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public async ValueTask<IEnumerable<SessionNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, WorldChunkRequestMessage message
    )
    {
        var chunk = _worldGeneratorService.GetOrGenerateChunk(message.Position.ToVector3Int());

        return new List<SessionNetworkMessage>
        {
            new(sessionId, new WorldChunkResponseMessage(chunk))
        };
    }
}
