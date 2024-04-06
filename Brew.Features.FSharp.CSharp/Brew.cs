using Brew.Features.FSharp.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;

namespace Brew.Features.FSharp.CSharp;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<Calculator>(x => new Calculator(FSharpOption<double>.None));
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var calculator = Host.Services.GetRequiredService<Calculator>();
        
        // Operations we can perform using F# functions
        (double value, Func<double, Calculator> op)[] operations = [(5, calculator.Add), (2, calculator.Subtract), (10, calculator.Multiply), (2, calculator.Divide)];
        
        foreach(var (n, op) in operations)
        {
            calculator = op(n);
            Logger.LogInformation("Operation: {MethodName}, Value: {N}, Result: {CalculatorCurrentValue}", op.Method.Name, n, calculator.CurrentValue);
        }
        
        return Task.CompletedTask;
    }
}