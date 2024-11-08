using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Core.Server.Interfaces.Services.System;
using NLua;

namespace Elderforge.Server.ScriptModules;

[ScriptModule]
public class ScriptModule
{
    private readonly IScriptEngineService _scriptEngineService;

    private readonly DirectoriesConfig _directoriesConfig;

    public ScriptModule(
        IScriptEngineService scriptEngineService, DirectoriesConfig directoryConfig
    )
    {
        _scriptEngineService = scriptEngineService;
        _directoriesConfig = directoryConfig;
    }


    [ScriptFunction("on_bootstrap", "Called when the server is bootstrapping")]
    public void RegisterBootstrap(LuaFunction function)
    {
        _scriptEngineService.AddContextVariable("bootstrap", function);
    }

    [ScriptFunction("gen_lua_def", "Generate lua definitions")]
    public string GenerateLuaDefinitions()
    {
        return _scriptEngineService.GenerateDefinitionsAsync().Result;
    }
}
