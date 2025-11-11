using Brew.Features.FSharp.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;

namespace Brew.Features.FSharp.CSharp;

/// <summary>
/// Demonstrates F#/C# interoperability: calling F# code from C#, working with F# types, and handling F# options
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<Calculator>(x => new Calculator(FSharpOption<double>.None));
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== F#/C# INTEROPERABILITY DEMONSTRATION ==========");
        Logger.LogInformation("Scenario: Calling F# Calculator from C# with fluent chaining\n");

        DemonstrateBasicCalculation();
        DemonstrateFSharpOptions();
        DemonstrateErrorHandling();

        Logger.LogInformation("\n---------- BENEFITS OF F#/C# INTEROP ----------");
        Logger.LogInformation("✓ Leverage functional programming in F# for complex logic");
        Logger.LogInformation("✓ Seamless integration with existing C# codebases");
        Logger.LogInformation("✓ Use F# pattern matching and immutability alongside C# OOP");
        Logger.LogInformation("✓ FSharpOption<T> provides safer null handling than C# nullables");
        Logger.LogInformation("✓ Fluent chaining works naturally across language boundaries");

        return Task.CompletedTask;
    }

    private void DemonstrateBasicCalculation()
    {
        Logger.LogInformation("---------- Basic Calculator Operations (Fluent API) ----------");

        var calculator = Host.Services.GetRequiredService<Calculator>();

        // Chain operations using F# methods
        calculator.Add(10)
            .Subtract(3)
            .Multiply(2)
            .Divide(4);

        Logger.LogInformation("  (10 + 0 - 3) * 2 / 4 = {Result}", calculator.CurrentValue);
        Logger.LogInformation("  F# object state maintained across C# method calls");
    }

    private void DemonstrateFSharpOptions()
    {
        Logger.LogInformation("\n---------- F# Option Handling from C# ----------");

        // None option (F# equivalent of null, but type-safe)
        var calcNone = new Calculator(FSharpOption<double>.None);
        Logger.LogInformation("  Calculator(None) initial value: {Value}", calcNone.CurrentValue);

        // Some option (F# equivalent of non-null value)
        var calcSome = new Calculator(FSharpOption<double>.Some(100.0));
        Logger.LogInformation("  Calculator(Some(100)) initial value: {Value}", calcSome.CurrentValue);

        Logger.LogInformation("  FSharpOption prevents accidental null references at compile time");
    }

    private void DemonstrateErrorHandling()
    {
        Logger.LogInformation("\n---------- Error Handling Across Languages ----------");

        var calculator = new Calculator(FSharpOption<double>.Some(20.0));

        try
        {
            calculator.Divide(0); // F# throws exception
        }
        catch (Exception ex)
        {
            Logger.LogWarning("  Division by zero handled: {Message}", ex.Message);
            Logger.LogInformation("  F# exceptions propagate naturally to C# try/catch");
        }
    }
}
