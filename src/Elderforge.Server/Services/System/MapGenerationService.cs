using Elderforge.Core.Server.Interfaces.Services.System;
using Serilog;

namespace Elderforge.Server.Services.System;

public class MapGenerationService :  IMapGenerationService
{
    private readonly ILogger _logger = Log.Logger.ForContext<MapGenerationService>();



}
