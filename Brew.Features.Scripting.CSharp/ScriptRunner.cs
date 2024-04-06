using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Scripting.CSharp;

public class ScriptRunner(ILogger<ScriptRunner> logger, ModuleBase moduleBase) : IScriptRunner
{
    private readonly Script<double> _operationScript = CSharpScript.Create<double>(performOperation, ScriptOptions.Default.AddReferences(typeof(Brew).Assembly), globalsType: typeof(Globals));

    private const string performOperation = 
        """
        using System;

        public static double PerformOperation(double num1, double num2, string op)
        {
            switch(op)
            {
                case "+": return num1 + num2;
                case "-": return num1 - num2;
                case "*": return num1 * num2; 
                case "/":
                {
                    if (num2 == 0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero");
                    }
                    return num1 / num2;
                }
                default:
                  throw new ArgumentException("Invalid operator");
            }
        }

        return PerformOperation(Num1, Num2, Op);
           
        """;

    public async Task RunScriptAsync()
    {
        var script = @"
                using System;
                using Microsoft.Extensions.Logging;

                Logger.LogInformation(""Hello from dynamically executed code!"");
                return 5;
            ";

        try
        {
            var options = ScriptOptions
                    .Default
                    .AddReferences(Assembly.GetExecutingAssembly())
                    .AddReferences("System")
                    .AddImports("Microsoft.Extensions.Logging")
                    .AddReferences("Microsoft.Extensions.Logging");
            
            var result = await CSharpScript.EvaluateAsync<int>(script, options, globals: new Globals { Logger = ModuleBase.Logger });

            logger.LogInformation("Script executed successfully with result: {Result}", result);
        }
        catch (CompilationErrorException e)
        {
            logger.LogError("Script compilation error: {EMessage}", e.Message);
        }
        catch (Exception e)
        {
            logger.LogError("Script execution error: {EMessage}", e.Message);
        }
    }

    public class Globals
    {
        public double Num1 { get; set; }
        public double Num2 { get; set; }
        public string Op { get; set; }
        public ILogger Logger { get; set; }
    }

    public async Task PerformOperation(double num1, double num2, string op)
    {
        try
        {
            ScriptState<double>? result = await _operationScript.RunAsync(globals: new Globals { Num1 = num1, Num2 = num2, Op = op });
            logger.LogInformation("Script executed successfully with ({Num1} {Op} {Num2})  result: {Result}", num1, op, num2, result.ReturnValue);
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