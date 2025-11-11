using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Singleton;

/// <summary>
/// Demonstrates the Singleton Pattern - ensuring only ONE instance of a class exists
/// throughout the application lifetime.
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // No services needed for this demo
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== SINGLETON PATTERN DEMONSTRATION ===\n");

        // BEFORE: Regular class - multiple instances
        Logger.LogInformation("--- BEFORE (Regular Class - Multiple Instances) ---");
        Logger.LogInformation("Creating first Person instance...");
        await Task.Delay(10); // Small delay to show different timestamps
        Person person1 = new Person();
        person1.Speak();

        Logger.LogInformation("\nCreating second Person instance...");
        await Task.Delay(10);
        Person person2 = new Person();
        person2.Speak();

        Logger.LogInformation("\nCreating third Person instance...");
        await Task.Delay(10);
        Person person3 = new Person();
        person3.Speak();

        Logger.LogInformation("\nNotice: Each Person has a DIFFERENT ID and creation time");
        Logger.LogInformation("Problem: Multiple instances consume more memory and may cause inconsistent state\n");

        // AFTER: Singleton - single instance
        Logger.LogInformation("--- AFTER (Singleton Pattern - Single Instance) ---");
        Logger.LogInformation("Accessing SingletonPerson.Instance for the first time...");
        SingletonPerson singleton1 = SingletonPerson.Instance;
        singleton1.Speak();

        Logger.LogInformation("\nAccessing SingletonPerson.Instance again...");
        await Task.Delay(50); // Even with delay, we get the same instance
        SingletonPerson singleton2 = SingletonPerson.Instance;
        singleton2.Speak();

        Logger.LogInformation("\nAccessing SingletonPerson.Instance a third time...");
        await Task.Delay(50);
        SingletonPerson singleton3 = SingletonPerson.Instance;
        singleton3.Speak();

        Logger.LogInformation("\nVerifying all references point to the SAME instance:");
        Logger.LogInformation($"singleton1 == singleton2: {ReferenceEquals(singleton1, singleton2)}");
        Logger.LogInformation($"singleton2 == singleton3: {ReferenceEquals(singleton2, singleton3)}");
        Logger.LogInformation($"singleton1 == singleton3: {ReferenceEquals(singleton1, singleton3)}");

        Logger.LogInformation("\nBenefit: Only ONE instance exists, ensuring consistent state and reduced memory usage");
        Logger.LogInformation("Use Case: Database connections, configuration managers, logging services, caches");
    }
}