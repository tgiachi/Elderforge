using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Elderforge.Core.Server.Attributes.Scripts;
using Elderforge.Core.Server.Data.Directories;
using Elderforge.Core.Server.Data.Scripts;
using Elderforge.Core.Server.Interfaces.Services;
using Elderforge.Core.Server.Types;
using Elderforge.Core.Server.Utils;
using NLua;
using NLua.Exceptions;
using Serilog;

namespace Elderforge.Server.Services;

public class ScriptEngineService : IScriptEngineService
{
    private readonly ILogger _logger = Log.ForContext<ScriptEngineService>();

    private readonly Lua _luaEngine;

    private readonly List<ScriptClassData> _scriptModules;
    private readonly DirectoriesConfig _directoryConfig;
    private readonly IServiceProvider _container;
    private const string _fileExtension = "*.lua";

    private const string _prefixToIgnore = "__";

    public List<ScriptFunctionDescriptor> Functions { get; } = new();
    public Dictionary<string, object> ContextVariables { get; } = new();

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ScriptEngineService(
        DirectoriesConfig directoryConfig, List<ScriptClassData> scriptModules, IServiceProvider container,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        _directoryConfig = directoryConfig;
        _scriptModules = scriptModules;
        _container = container;
        _jsonSerializerOptions = jsonSerializerOptions;
        _luaEngine = new Lua();

        _luaEngine.LoadCLRPackage();

        AddModulesDirectory();
    }

    public async Task StartAsync()
    {
        await ScanScriptModulesAsync();
        var scriptsToLoad = Directory.GetFiles(
            _directoryConfig[DirectoryType.Scripts],
            _fileExtension,
            SearchOption.AllDirectories
        );

        foreach (var script in scriptsToLoad)
        {
            var fileName = Path.GetFileName(script);

            if (!fileName.StartsWith(_prefixToIgnore))
            {
                await ExecuteFileAsync(script);
            }
        }


        if (ContextVariables.TryGetValue("bootstrap", out object? value) && value is LuaFunction bootstrap)
        {
            bootstrap.Call();
        }
    }

    private Task ScanScriptModulesAsync()
    {
        foreach (var module in _scriptModules)
        {
            _logger.Debug("Found script module {Module}", module.ClassType.Name);

            try
            {
                var obj = _container.GetService(module.ClassType);

                foreach (var scriptMethod in module.ClassType.GetMethods())
                {
                    var sMethodAttr = scriptMethod.GetCustomAttribute<ScriptFunctionAttribute>();

                    if (sMethodAttr == null)
                    {
                        continue;
                    }

                    ExtractFunctionDescriptor(sMethodAttr, scriptMethod);

                    _logger.Debug("Adding script method {M}", sMethodAttr.Alias ?? scriptMethod.Name);

                    _luaEngine[sMethodAttr.Alias ?? scriptMethod.Name] = CreateLuaEngineDelegate(obj, scriptMethod);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error during initialize script module {Alias}: {Ex}", module.ClassType, ex);
            }
        }

        return Task.CompletedTask;
    }

    public async Task ExecuteFileAsync(string file)
    {
        _logger.Information("Executing script: {File}", Path.GetFileName(file));
        try
        {
            var script = await File.ReadAllTextAsync(file);
            _luaEngine.DoString(script);
        }
        catch (LuaException ex)
        {
            _logger.Error(ex, "Error executing script: {File}: {Formatted}", Path.GetFileName(file), FormatException(ex));
        }
    }

    private void ExtractFunctionDescriptor(ScriptFunctionAttribute attribute, MethodInfo methodInfo)
    {
        var descriptor = new ScriptFunctionDescriptor
        {
            FunctionName = attribute.Alias ?? methodInfo.Name,
            Help = attribute.Help,
            Parameters = new(),
            ReturnType = methodInfo.ReturnType.Name,
            RawReturnType = methodInfo.ReturnType
        };

        foreach (var parameter in methodInfo.GetParameters())
        {
            descriptor.Parameters.Add(
                new ScriptFunctionParameterDescriptor(
                    parameter.Name,
                    parameter.ParameterType.Name,
                    parameter.ParameterType
                )
            );
        }

        Functions.Add(descriptor);
    }

    public ScriptEngineExecutionResult ExecuteCommand(string command)
    {
        try
        {
            var result = new ScriptEngineExecutionResult
            {
                Result = _luaEngine.DoString(command)
            };

            return result;
        }
        catch (LuaException ex)
        {
            return new ScriptEngineExecutionResult { Exception = ex };
        }
    }

    public void AddContextVariable(string name, object value)
    {
        _logger.Information("Adding context variable {Name} with value {Value}", name, value);
        _luaEngine[name] = value;
        ContextVariables[name] = value;
    }

    public TVar? GetContextVariable<TVar>(string name, bool throwIfNotFound = true) where TVar : class
    {
        if (!ContextVariables.TryGetValue(name, out var ctxVar))
        {
            _logger.Error("Variable {Name} not found", name);

            if (throwIfNotFound)
            {
                throw new KeyNotFoundException($"Variable {name} not found");
            }

            return default;
        }

        var json = JsonSerializer.Serialize(ScriptUtils.LuaTableToDictionary((LuaTable)ctxVar), _jsonSerializerOptions);

        return JsonSerializer.Deserialize<TVar>(json, _jsonSerializerOptions);
    }


    private static Delegate CreateLuaEngineDelegate(object obj, MethodInfo method)
    {
        var parameterTypes =
            method.GetParameters().Select(p => p.ParameterType).Concat(new[] { method.ReturnType }).ToArray();
        return method.CreateDelegate(Expression.GetDelegateType(parameterTypes), obj);
    }

    public async Task<string> GenerateDefinitionsAsync()
    {
        var luaDefinitions = new StringBuilder();

        luaDefinitions.AppendLine("-- Elderforge Engine Lua Definitions");
        luaDefinitions.AppendLine();


        foreach (var constant in ContextVariables)
        {
            luaDefinitions.AppendLine(
                $"-- {constant.Key}: {CSharpLuaConverterUtils.ConvertCSharpTypeToLuaDef(constant.Value.GetType().Name)}"
            );
        }

        luaDefinitions.AppendLine();


        foreach (var function in Functions)
        {
            if (!string.IsNullOrEmpty(function.Help))
            {
                luaDefinitions.AppendLine($"-- {function.Help}");
            }

            luaDefinitions.Append($"function {function.FunctionName}(");

            for (int i = 0; i < function.Parameters.Count; i++)
            {
                var param = function.Parameters[i];
                luaDefinitions.Append($"{param.ParameterName}");

                if (i < function.Parameters.Count - 1)
                {
                    luaDefinitions.Append(", ");
                }
            }

            luaDefinitions.AppendLine(") end");
            luaDefinitions.AppendLine();
        }

        return luaDefinitions.ToString();
    }


    private void AddModulesDirectory()
    {
        var modulesPath = Path.Combine(_directoryConfig[DirectoryType.Scripts]) + Path.DirectorySeparatorChar;
        var scriptModulePath = Path.Combine(_directoryConfig[DirectoryType.ScriptModules]) + Path.DirectorySeparatorChar;


        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            modulesPath = modulesPath.Replace(@"\", @"\\");
            scriptModulePath = scriptModulePath.Replace(@"\", @"\\");
        }

        _luaEngine.DoString(
            $"""
             -- Update the search path
             local module_folder = '{modulesPath}'
             local module_script_folder = '{scriptModulePath}'
             package.path = module_folder .. '?.lua;' .. package.path
             package.path = module_script_folder .. '?.lua;' .. package.path
             """
        );
    }

    private static string FormatException(LuaException e)
    {
        var source = (string.IsNullOrEmpty(e.Source)) ? "<no source>" : e.Source[..^2];
        return string.Format("{0}\nLua (at {2})", e.Message, "", source);
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _luaEngine.Dispose();
    }
}
