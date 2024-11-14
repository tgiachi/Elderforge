using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Interfaces.Services.System;
using Elderforge.Server.Data;

namespace Elderforge.Server.ScriptModules;

[ScriptModule]
public class ElderforgeModule
{
    private readonly IScriptEngineService _scriptEngineService;

    public ElderforgeModule(IScriptEngineService scriptEngineService)
    {
        _scriptEngineService = scriptEngineService;
    }


    [ScriptFunction("set_game_cfg")]
    public void SetGameConfig(object config)
    {
        _scriptEngineService.AddContextVariable("gameConfig", config);
    }
}
