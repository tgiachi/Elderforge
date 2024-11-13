using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Interfaces.Services.Game;
using NLua;

namespace Elderforge.Server.ScriptModules;

[ScriptModule]
public class GameCommandModule
{
    private readonly IGameCommandService _gameCommandService;

    public GameCommandModule(IGameCommandService gameCommandService)
    {
        _gameCommandService = gameCommandService;
    }

    [ScriptFunction("register_cmd")]
    public void RegisterCommand(string command, string description, string help, LuaFunction action)
    {
        _gameCommandService.RegisterCommand(command, description, help, context => action.Call(context));

    }
}
