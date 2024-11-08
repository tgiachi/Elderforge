using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Core.Server.Interfaces.Services.System;

namespace Elderforge.Server.ScriptModules;


[ScriptModule]
public class VariableServiceModule
{

    private readonly IVariablesService _variablesService;

    public VariableServiceModule(IVariablesService variablesService)
    {
        _variablesService = variablesService;
    }


    [ScriptFunction("r_text")]
    public string ReplaceText(string text)
    {
        return _variablesService.TranslateText(text);
    }
}
