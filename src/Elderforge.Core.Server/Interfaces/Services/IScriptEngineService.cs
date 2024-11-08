using Elderforge.Core.Interfaces.Services.Base;
using Elderforge.Core.Server.Data.Scripts;

namespace Elderforge.Core.Server.Interfaces.Services;

public interface IScriptEngineService : IElderforgeService
{
    Task ExecuteFileAsync(string file);

    ScriptEngineExecutionResult ExecuteCommand(string command);

    List<ScriptFunctionDescriptor> Functions { get; }

    Dictionary<string, object> ContextVariables { get; }

    Task<string> GenerateDefinitionsAsync();

    void AddContextVariable(string name, object value);

    TVar? GetContextVariable<TVar>(string name, bool throwIfNotFound = true) where TVar : class;
}
