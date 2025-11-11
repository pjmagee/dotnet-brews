using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.ImmutableRecords;

/*
 * Immutable Records (C# 9+) provide value-based equality, immutability, and concise syntax.
 * 
 * Key Features:
 * - Value equality (compared by data, not reference)
 * - Positional deconstruction
 * - with-expressions for non-destructive mutation
 * - Compiler-generated ToString, GetHashCode, Equals
 * 
 * Benefits:
 * - Thread-safe by default (immutability)
 * - Predictable behavior in collections (value equality)
 * - Functional programming patterns
 * - Reduced boilerplate
 */
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("============== Immutable Records Demonstration ==============");
        Logger.LogInformation("Showcasing value equality, with-expressions, and deconstruction");
        Logger.LogInformation("==============================================================");
        Logger.LogInformation("");

        DemonstrateValueEquality();
        Logger.LogInformation("");

        DemonstrateWithExpressions();
        Logger.LogInformation("");

        DemonstrateDeconstruction();
        Logger.LogInformation("");

        DemonstrateThreadSafety();
        Logger.LogInformation("");

        Logger.LogInformation("======================= BENEFITS =========================");
        Logger.LogInformation("✓ Value equality: records compared by content, not reference");
        Logger.LogInformation("✓ Immutability: thread-safe, predictable state");
        Logger.LogInformation("✓ with-expressions: copy with modifications");
        Logger.LogInformation("✓ Less boilerplate: compiler generates Equals/GetHashCode/ToString");
        Logger.LogInformation("==========================================================");

        return Task.CompletedTask;
    }

    private void DemonstrateValueEquality()
    {
        Logger.LogInformation("----------- Value Equality (Records vs Classes) ----------");
        
        var person1 = new Person("Alice", new DateTime(1990, 5, 15));
        var person2 = new Person("Alice", new DateTime(1990, 5, 15));
        var person3 = new Person("Bob", new DateTime(1985, 3, 20));

        Logger.LogInformation("person1: {Person}", person1);
        Logger.LogInformation("person2: {Person}", person2);
        Logger.LogInformation("person3: {Person}", person3);
        Logger.LogInformation("person1 == person2: {Equal} (same data → true)", person1 == person2);
        Logger.LogInformation("person1 == person3: {Equal} (different data → false)", person1 == person3);

        var class1 = new MutablePersonClass("Alice", new DateTime(1990, 5, 15));
        var class2 = new MutablePersonClass("Alice", new DateTime(1990, 5, 15));
        Logger.LogInformation("class1 == class2: {Equal} (reference equality → false!)", class1 == class2);
    }

    private void DemonstrateWithExpressions()
    {
        Logger.LogInformation("------------- with-Expressions (Non-Destructive) ---------");
        
        var original = new Person("Alice", new DateTime(1990, 5, 15));
        Logger.LogInformation("Original: {Person}", original);

        var modified = original with { Name = "Alicia" };
        Logger.LogInformation("Modified (with new name): {Person}", modified);
        Logger.LogInformation("Original unchanged: {Person}", original);
        Logger.LogInformation("Records are immutable; 'with' creates a new instance!");
    }

    private void DemonstrateDeconstruction()
    {
        Logger.LogInformation("----------------- Deconstruction ---------------------");
        
        var person = new Person("Charlie", new DateTime(1995, 8, 10));
        var (name, dob) = person; // Positional deconstruction
        
        Logger.LogInformation("Deconstructed: Name={Name}, DOB={DOB:yyyy-MM-dd}", name, dob);
        Logger.LogInformation("Convenient for LINQ, pattern matching, assignments");
    }

    private void DemonstrateThreadSafety()
    {
        Logger.LogInformation("------------------ Thread Safety ---------------------");
        
        var sharedRecord = new Person("Dave", new DateTime(2000, 1, 1));
        Logger.LogInformation("Shared record: {Person}", sharedRecord);
        
        // Immutable = thread-safe, no locking needed
        var tasks = Enumerable.Range(0, 5).Select(i => Task.Run(() =>
        {
            var local = sharedRecord with { Name = $"Thread{i}" };
            Logger.LogInformation("  Thread {Id} created: {Person}", i, local);
        }));

        Task.WaitAll(tasks.ToArray());
        Logger.LogInformation("Original still unchanged: {Person}", sharedRecord);
        Logger.LogInformation("No race conditions, no locks required!");
    }
}