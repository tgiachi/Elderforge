using Elderforge.Core.Interfaces.Services.Base;
using Elderforge.Core.Server.Serialization.Map;
using Elderforge.Shared.Chunks;

namespace Elderforge.Core.Server.Interfaces.Services.System;

public interface IMapGenerationService : IElderforgeService
{
    Task<List<ChunkEntity>> GenerateMapAsync(int width, int height);

    Task<MapSerializable> GenerateMapSerializableAsync(int width, int height, List<ChunkEntity> chunks);

}
