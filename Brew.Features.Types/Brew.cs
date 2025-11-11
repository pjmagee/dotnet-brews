using System.CodeDom;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Types;

/// <summary>
/// Demonstrates C# type system introspection:
/// 1. Mapping C# built-in aliases (int, string, etc.) to their underlying CLR types.
/// 2. Basic reflection over a sample class (properties, methods, metadata).
/// 3. Foundation for serializers, ORMs, DI containers, and code generation.
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<CSharpCodeProvider>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== C# TYPE SYSTEM EXPLORATION ==========");
        Logger.LogInformation("Scenario: Discover type aliases and introspect .NET type system\n");

        DemonstrateTypeAliases();
        DemonstrateReflectionBasics();

        Logger.LogInformation("\n---------- BENEFITS OF TYPE INTROSPECTION ----------");
        Logger.LogInformation("✓ Understand relationship between C# aliases and CLR types");
        Logger.LogInformation("✓ Runtime type discovery and metadata inspection");
        Logger.LogInformation("✓ Foundation for serializers, ORMs, DI containers");
        Logger.LogInformation("✓ Enables dynamic code generation and analysis");
        Logger.LogInformation("✓ Powerful debugging and diagnostic capabilities");

        return Task.CompletedTask;
    }

    private void DemonstrateTypeAliases()
    {
        Logger.LogInformation("---------- C# Type Aliases → CLR Types ----------");

        var provider = Host.Services.GetRequiredService<CSharpCodeProvider>();
        var mscorlib = Assembly.GetAssembly(typeof(int))!;

        var aliases = mscorlib.DefinedTypes
            .Where(t => t.Namespace == nameof(System))
            .Select(type => new { Type = type, Alias = provider.GetTypeOutput(new CodeTypeReference(type)) })
            .Where(x => !x.Alias.Contains('.')) // Only simple aliases
            .OrderBy(x => x.Alias)
            .Take(15)
            .ToList();

        Logger.LogInformation("  Common C# Aliases:");
        foreach (var item in aliases)
        {
            Logger.LogInformation("    {Alias,-10} → {Type}", item.Alias, item.Type.Name);
        }

        Logger.LogInformation("  Note: 'int' is alias for System.Int32, 'string' for System.String, etc.");
    }

    private void DemonstrateReflectionBasics()
    {
        Logger.LogInformation("\n---------- Reflection: Runtime Type Inspection ----------");

        var sampleType = typeof(SampleClass);
        Logger.LogInformation("  Inspecting type: {Type}", sampleType.Name);
        Logger.LogInformation("  Namespace: {Namespace}", sampleType.Namespace);
        Logger.LogInformation("  Assembly: {Assembly}", sampleType.Assembly.GetName().Name);
        Logger.LogInformation("  Is Class: {IsClass}, Is Value Type: {IsValueType}", sampleType.IsClass, sampleType.IsValueType);

        Logger.LogInformation("\n  Properties:");
        foreach (var prop in sampleType.GetProperties())
        {
            Logger.LogInformation("    {Name} ({Type})", prop.Name, prop.PropertyType.Name);
        }

        Logger.LogInformation("\n  Methods:");
        foreach (var method in sampleType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Logger.LogInformation("    {ReturnType} {MethodName}({Parameters})", method.ReturnType.Name, method.Name, parameters);
        }
    }
}

/// <summary>
/// Sample class used for reflection demonstration.
/// </summary>
public class SampleClass
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public void DoSomething(string input) { }
    public int Calculate(int a, int b) => a + b;
}
