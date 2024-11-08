using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Core.Server.Interfaces.Services.System;

namespace Elderforge.Server.ScriptModules;

[ScriptModule]
public class ContextVariableModule
{
    private readonly IScriptEngineService _scriptEngineService;

    public ContextVariableModule(IScriptEngineService scriptEngineService)
    {
        _scriptEngineService = scriptEngineService;
    }

    [ScriptFunction("add_var")]
    public void AddContextVariable(string variableName, object value)
    {
        _scriptEngineService.AddContextVariable(variableName, value);
    }
}
