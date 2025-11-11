using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.PatternMatching;

/*
 * Pattern Matching (C# 7.0+) enables sophisticated type/value checks in a concise, readable way.
 * 
 * Pattern Types:
 * - Type patterns: is Type, switch with types
 * - Property patterns: { Property: value }
 * - Relational patterns: >, <, >=, <=
 * - Logical patterns: and, or, not
 * - List patterns: [item1, item2, ..]
 * 
 * Benefits:
 * - Eliminates verbose if-else chains
 * - Exhaustive matching with compiler warnings
 * - More readable domain logic
 * - Type safety
 */
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("============ C# Pattern Matching Demonstration ============");
        Logger.LogInformation("Showcasing type, property, relational, and list patterns");
        Logger.LogInformation("============================================================");
        Logger.LogInformation("");

        DemonstrateTypePatterns();
        Logger.LogInformation("");

        DemonstratePropertyPatterns();
        Logger.LogInformation("");

        DemonstrateRelationalPatterns();
        Logger.LogInformation("");

        DemonstrateListPatterns();
        Logger.LogInformation("");

        DemonstrateLogicalPatterns();
        Logger.LogInformation("");

        Logger.LogInformation("======================= BENEFITS =======================");
        Logger.LogInformation("✓ Concise: replaces verbose if-else chains");
        Logger.LogInformation("✓ Type-safe: compiler enforces exhaustive matching");
        Logger.LogInformation("✓ Readable: expresses intent clearly");
        Logger.LogInformation("✓ Powerful: combines type, value, and logical checks");
        Logger.LogInformation("========================================================");

        return Task.CompletedTask;
    }

    private void DemonstrateTypePatterns()
    {
        Logger.LogInformation("--------------- Type Patterns (is/switch) --------------");

        object obj1 = "Hello, World!";
        object obj2 = 42;
        object obj3 = new Hero { Name = "Superman", Age = 30 };

        ProcessObject(obj1);
        ProcessObject(obj2);
        ProcessObject(obj3);
    }

    private void ProcessObject(object obj)
    {
        var result = obj switch
        {
            string s => $"String with length {s.Length}",
            int i => $"Integer: {i}",
            Hero { Name: var name } => $"Hero: {name}",
            _ => "Unknown type"
        };
        Logger.LogInformation("  {Result}", result);
    }

    private void DemonstratePropertyPatterns()
    {
        Logger.LogInformation("---------- Property Patterns (Object Matching) ---------");

        var heroes = new[]
        {
            new Hero { Name = "Superman", Age = 35, Power = "Flight" },
            new Hero { Name = "Batman", Age = 40, Power = null },
            new Hero { Name = "Wonder Woman", Age = 5000, Power = "Super Strength" }
        };

        foreach (var hero in heroes)
        {
            var classification = hero switch
            {
                { Name: "Superman", Power: "Flight" } => "Man of Steel",
                { Name: "Batman", Power: null } => "Dark Knight (no powers)",
                { Age: > 100 } => "Ancient hero",
                _ => "Regular hero"
            };
            Logger.LogInformation("  {Name}: {Classification}", hero.Name, classification);
        }
    }

    private void DemonstrateRelationalPatterns()
    {
        Logger.LogInformation("---------- Relational Patterns (>, <, and, or) --------");

        var ages = new[] { 5, 15, 25, 65, 100 };

        foreach (var age in ages)
        {
            var category = age switch
            {
                < 13 => "Child",
                >= 13 and < 20 => "Teenager",
                >= 20 and < 60 => "Adult",
                >= 60 and < 100 => "Senior",
                _ => "Centenarian (100+)"
            };
            Logger.LogInformation("  Age {Age}: {Category}", age, category);
        }
    }

    private void DemonstrateListPatterns()
    {
        Logger.LogInformation("------------- List Patterns (Array Matching) -----------");

        var lists = new[]
        {
            new[] { 1 },
            new[] { 1, 2 },
            new[] { 1, 2, 3 },
            new[] { 1, 2, 3, 4, 5 }
        };

        foreach (var list in lists)
        {
            var description = list switch
            {
                [] => "Empty",
                [var x] => $"Single element: {x}",
                [var x, var y] => $"Two elements: {x}, {y}",
                [var x, .., var y] => $"First: {x}, Last: {y}, Length: {list.Length}",
            };
            Logger.LogInformation("  {Description}", description);
        }
    }

    private void DemonstrateLogicalPatterns()
    {
        Logger.LogInformation("------------ Logical Patterns (not/and/or) -------------");

        var values = new object?[] { null, 0, 5, 10, 15, "text" };

        foreach (var value in values)
        {
            var check = value switch
            {
                null => "Null value",
                not int => "Not an integer",
                < 0 or > 10 => $"Out of range: {value}",
                _ => $"In range [0-10]: {value}"
            };
            Logger.LogInformation("  {Value}: {Check}", value ?? "null", check);
        }
    }
}