using System.Text;
using Eldergrove.Engine.Core.Attributes.Scripts;
using Eldergrove.Engine.Core.Data.Internal;
using Eldergrove.Engine.Core.Interfaces.Services;
using NLua;


namespace Eldergrove.Engine.Core.ScriptsModules;

[ScriptModule]
public class ScriptModule
{
    private readonly IScriptEngineService _scriptEngineService;

    private readonly DirectoryConfig _directoryConfig;
    private readonly IDataLoaderService _dataLoaderService;

    public ScriptModule(
        IScriptEngineService scriptEngineService, IDataLoaderService dataLoaderService, DirectoryConfig directoryConfig
    )
    {
        _scriptEngineService = scriptEngineService;
        _dataLoaderService = dataLoaderService;
        _directoryConfig = directoryConfig;
    }

    [ScriptFunction("add_ctx")]
    public void AddContextVariable(string name, object value)
    {
        _scriptEngineService.AddContextVariable(name, value);
    }


    [ScriptFunction("load_json", "Load json file")]
    public void LoadJson(string path)
    {
        _dataLoaderService.LoadData(Path.Combine(_directoryConfig.RootDirectory, path));
    }

    [ScriptFunction("on_bootstrap")]
    public void RegisterBootstrap(LuaFunction function)
    {
        _scriptEngineService.AddContextVariable("bootstrap", function);
    }


    [ScriptFunction("game_config", "Set game config")]
    public void SetGameConfig(object value)
    {
        _scriptEngineService.AddContextVariable("game_config", value);
    }


    [ScriptFunction("gen_lua_def", "Generate lua definitions")]
    public string GenerateLuaDefinitions()
    {
        return _scriptEngineService.GenerateDefinitionsAsync().Result;
    }
}
