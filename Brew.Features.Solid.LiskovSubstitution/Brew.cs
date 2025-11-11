using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.LiskovSubstitution;

/// <summary>
/// Demonstrates the Liskov Substitution Principle using the classic Rectangle/Square example.
/// 
/// LSP states: Objects of a superclass should be replaceable with objects of a subclass without breaking the application.
/// In other words, subtypes must be behaviorally substitutable for their base types.
/// 
/// The Rectangle/Square problem:
/// - Mathematically, a square IS-A rectangle (special case where width == height)
/// - In OOP, Square inheriting from Rectangle violates LSP because:
///   1. Square has an invariant: width must always equal height
///   2. Rectangle's contract allows width and height to be set independently
///   3. Square breaks this contract by coupling the properties
///   4. Client code expecting Rectangle behavior breaks when given a Square
/// 
/// This demonstrates that inheritance should be based on behavioral compatibility, not mathematical relationships.
/// </summary>
public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== BEFORE (VIOLATES LSP) ===");
        Logger.LogInformation("Square inherits from Rectangle, but breaks the behavioral contract:");
        Logger.LogInformation("");

        // Client code expects Rectangle behavior: width and height can be set independently
        Before.Rectangle rect = new Before.Rectangle { Width = 5, Height = 10 };
        Logger.LogInformation("Rectangle: {Width}x{Height}, Area: {Area}", rect.Width, rect.Height, rect.GetArea());
        Logger.LogInformation("✓ Expected: 5x10 = 50");
        Logger.LogInformation("");

        // Same client code with Square - behavior is broken!
        Before.Rectangle square = new Before.Square();
        square.Width = 5;   // Also sets Height to 5 (hidden side effect)
        square.Height = 10; // Also sets Width to 10 (breaks client expectation)
        Logger.LogInformation("Square as Rectangle: {Width}x{Height}, Area: {Area}", square.Width, square.Height, square.GetArea());
        Logger.LogInformation("✗ Expected: 5x10 = 50, but got 10x10 = 100");
        Logger.LogInformation("✗ Setting Width affected Height (violated Rectangle's contract)");
        Logger.LogInformation("✗ Client code cannot substitute Square for Rectangle without breaking");
        Logger.LogInformation("");
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== AFTER (FOLLOWS LSP) ===");
        Logger.LogInformation("Both Rectangle and Square implement Shape independently:");
        Logger.LogInformation("");

        // Client code works with Shape abstraction
        List<After.Shape> shapes =
        [
            new After.RectangleShape { Width = 5, Height = 10 },
            new After.SquareShape { Side = 7 }
        ];

        foreach (var shape in shapes)
        {
            Logger.LogInformation("{Description}: Area = {Area}", shape.GetDescription(), shape.GetArea());
        }

        Logger.LogInformation("");
        Logger.LogInformation("✓ Both shapes can be substituted for Shape without breaking behavior");
        Logger.LogInformation("✓ Each shape maintains its own invariants without violating the abstraction's contract");
        Logger.LogInformation("✓ Client code works correctly with any Shape implementation");
        
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("");
        return Task.CompletedTask;
    }
}