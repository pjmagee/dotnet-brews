using System.Dynamic;
using Microsoft.Extensions.Logging;

namespace Brew.Features.DynamicRuntime.Dynamic;

// Demonstrates ExpandoObject: dynamic object with runtime-added properties/methods
public class ExpandoObjectDemo(ILogger<ExpandoObjectDemo> logger)
{
    public void DemonstrateExpandoObject()
    {
        logger.LogInformation("----------- ExpandoObject (Runtime Member Addition) ----------");

        dynamic expando = new ExpandoObject();

        // Add properties at runtime
        expando.Name = "Duck";
        expando.Type = "Bird";
        logger.LogInformation("  Created: Name={Name}, Type={Type}", (object)expando.Name, (object)expando.Type);

        // Add methods at runtime
        expando.Quack = new Action(() => logger.LogInformation("  Quack quack!"));
        expando.Quack();

        // Change type on the fly
        expando.Number = 42;
        logger.LogInformation("  Number (int): {Number}", (object)expando.Number);
        expando.Number = 3.14m;
        logger.LogInformation("  Number (decimal): {Number}", (object)expando.Number);

        // Add function with return value
        expando.AskPermission = new Func<string, bool>(message =>
        {
            var granted = Random.Shared.Next(0, 2) == 1;
            logger.LogInformation("  {Message} -> {Status}", message, granted ? "GRANTED" : "DENIED");
            return granted;
        });

        expando.AskPermission("May I proceed?");

        logger.LogInformation("USE CASE: JSON deserialization, COM interop, scripting");
    }
}

// Compares static typing vs dynamic typing
public class DynamicVsStaticDemo(ILogger<DynamicVsStaticDemo> logger)
{
    public void CompareApproaches()
    {
        logger.LogInformation("---------- Static vs Dynamic Typing Comparison -----------");

        // Static approach: compile-time safety
        var staticPerson = new Person { Name = "Alice", Age = 30 };
        logger.LogInformation("  [STATIC] {Name} is {Age} years old", staticPerson.Name, staticPerson.Age);
        // staticPerson.InvalidProperty = 123; // <- Compile error (good!)

        // Dynamic approach: runtime flexibility
        dynamic dynamicPerson = new ExpandoObject();
        dynamicPerson.Name = "Bob";
        dynamicPerson.Age = 25;
        logger.LogInformation("  [DYNAMIC] {Name} is {Age} years old", (object)dynamicPerson.Name, (object)dynamicPerson.Age);

        // Can add arbitrary properties (no compile check)
        dynamicPerson.FavoriteColor = "Blue";
        logger.LogInformation("  [DYNAMIC] Added new property at runtime: {Color}", (object)dynamicPerson.FavoriteColor);

        // Runtime error example (commented to avoid crash)
        // var oops = dynamicPerson.NonExistentProperty; // <- RuntimeBinderException!

        logger.LogInformation("TRADE-OFF: Static = safety, Dynamic = flexibility");
    }

    private class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}

// Custom DynamicObject for specialized behavior
public class DynamicObjectDemo(ILogger<DynamicObjectDemo> logger)
{
    public void ShowCustomDynamicBehavior()
    {
        logger.LogInformation("---------- Custom DynamicObject (Override Behavior) -------");

        dynamic smartDict = new SmartDictionary();
        smartDict.Foo = "Bar";
        smartDict.Count = 42;

        logger.LogInformation("  smartDict.Foo: {Value}", (object?)smartDict.Foo ?? "null");
        logger.LogInformation("  smartDict.Count: {Value}", (object?)smartDict.Count ?? "null");
        logger.LogInformation("  smartDict.Missing (auto-default): {Value}", (object?)smartDict.Missing ?? "null");

        logger.LogInformation("USE CASE: DSLs, fluent APIs with unknown members");
    }

    private class SmartDictionary : DynamicObject
    {
        private readonly Dictionary<string, object?> _data = new();

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            // Return value if exists, otherwise null (no exception)
            return _data.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            _data[binder.Name] = value;
            return true;
        }
    }
}
