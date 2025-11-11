using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.HiddenKeywords;

// Demonstrates __arglist (legacy variable-length arguments) vs modern params
public class ArgListDemo(ILogger<ArgListDemo> logger)
{
    public void ModernApproach()
    {
        logger.LogInformation("[MODERN] Using 'params' keyword (recommended):");
        PrintModern(1, 2, 3);
        PrintModern("Hello", "World", 42, true);
        PrintModern(); // empty is fine
    }

    private void PrintModern(params object[] args)
    {
        logger.LogInformation("  Received {Count} args: {Args}", args.Length, string.Join(", ", args));
    }

    public void LegacyApproach()
    {
        logger.LogInformation("[LEGACY] Using '__arglist' (not recommended):");
        PrintLegacy(__arglist(1, "2", 3.0, true, DateTime.Now));
    }

    // __arglist: accepts variable typed arguments without type safety
    private void PrintLegacy(__arglist)
    {
        ArgIterator iterator = new ArgIterator(__arglist);
        List<object> items = new();

        while (iterator.GetRemainingCount() > 0)
        {
            var arg = iterator.GetNextArg();
            items.Add(TypedReference.ToObject(arg)!);
        }

        logger.LogInformation("  Received {Count} args: {Args}", items.Count, string.Join(", ", items));
        logger.LogInformation("  WARNING: No compile-time type safety!");
    }
}
