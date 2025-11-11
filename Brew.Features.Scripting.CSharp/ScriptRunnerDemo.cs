using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Scripting.CSharp;

public class ScriptRunnerDemo(ILogger<ScriptRunnerDemo> logger)
{
    private readonly ScriptOptions _scriptOptions = ScriptOptions.Default
        .AddReferences(typeof(Brew).Assembly)
        .AddReferences(Assembly.GetExecutingAssembly())
        .AddReferences(typeof(ILogger).Assembly)
        .AddImports("System")
        .AddImports("Microsoft.Extensions.Logging");

    public class Globals
    {
        public double Num1 { get; set; }
        public double Num2 { get; set; }
        public string Op { get; set; } = string.Empty;
        public ILogger Logger { get; set; } = null!;
    }

    public async Task RunScriptAsync(string scriptFileName)
    {
        try
        {
            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", scriptFileName);
            
            if (!File.Exists(scriptPath))
            {
                logger.LogError("Script file not found: {ScriptPath}", scriptPath);
                return;
            }

            var scriptCode = await File.ReadAllTextAsync(scriptPath);
            
            var result = await CSharpScript.EvaluateAsync<string>(
                scriptCode, 
                _scriptOptions, 
                globals: new Globals { Logger = logger });

            logger.LogInformation("Script '{ScriptName}' result: {Result}", scriptFileName, result);
        }
        catch (CompilationErrorException e)
        {
            logger.LogError("Script compilation error: {Message}", e.Message);
        }
        catch (Exception e)
        {
            logger.LogError("Script execution error: {Message}", e.Message);
        }
    }

    public async Task PerformOperation(double num1, double num2, string op)
    {
        try
        {
            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "calculator.csx");
            
            if (!File.Exists(scriptPath))
            {
                logger.LogError("Calculator script not found: {ScriptPath}", scriptPath);
                return;
            }

            var scriptCode = await File.ReadAllTextAsync(scriptPath);
            
            var result = await CSharpScript.EvaluateAsync<double>(
                scriptCode,
                _scriptOptions,
                globals: new Globals { Num1 = num1, Num2 = num2, Op = op, Logger = logger });

            logger.LogInformation("Operation result: {Result}", result);
        }
        catch (CompilationErrorException e)
        {
            logger.LogError("Script compilation error: {Message}", e.Message);
        }
        catch (Exception e)
        {
            logger.LogError("Script execution error: {Message}", e.Message);
        }
    }
}