using System.Reflection;
using Brew;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var command = args.Length > 0 ? args[0].ToLowerInvariant() : "all";
var brewName = args.Length > 1 ? args[1] : null;

CancellationTokenSource source = new CancellationTokenSource();

Console.CancelKeyPress += (sender, args) =>
{
    args.Cancel = true;
    source.Cancel();
};

using(var host = ConfigureBuilder().Build())
{
    var brews = host.Services.GetServices<IBrew>().ToList();

    switch (command)
    {
        case "list":
            Console.WriteLine("Available Brews:");
            Console.WriteLine("================");
            foreach (var brew in brews.OrderBy(b => b.GetType().FullName))
            {
                var typeName = brew.GetType().FullName ?? brew.GetType().Name;
                var description = brew.Description ?? "No description available";
                Console.WriteLine($"  {typeName}");
                Console.WriteLine($"    {description}");
                Console.WriteLine();
            }
            break;

        case "run":
            if (string.IsNullOrEmpty(brewName))
            {
                Console.Error.WriteLine("Error: Brew name required for 'run' command");
                Console.Error.WriteLine("Usage: Brew.Console run <brew-name-pattern>");
                Console.Error.WriteLine("Examples:");
                Console.Error.WriteLine("  Brew.Console run CQRS.Simple");
                Console.Error.WriteLine("  Brew.Console run Factory");
                Console.Error.WriteLine("  Brew.Console run Mef");
                Environment.Exit(1);
            }

            // Find brews matching the pattern (case-insensitive)
            var matchingBrews = brews.Where(b =>
            {
                var fullName = b.GetType().FullName ?? "";
                var typeName = b.GetType().Name;
                
                // Match against type name or any part of the full name containing the pattern
                return fullName.Contains(brewName, StringComparison.OrdinalIgnoreCase) ||
                       typeName.Equals(brewName, StringComparison.OrdinalIgnoreCase);
            }).ToList();

            if (matchingBrews.Count == 0)
            {
                Console.Error.WriteLine($"Error: No brews found matching '{brewName}'");
                Console.Error.WriteLine("Use 'list' command to see available brews");
                Environment.Exit(1);
            }

            if (matchingBrews.Count > 1)
            {
                Console.Error.WriteLine($"Error: Multiple brews match '{brewName}':");
                foreach (var brew in matchingBrews.OrderBy(b => b.GetType().FullName))
                {
                    Console.Error.WriteLine($"  - {brew.GetType().FullName}");
                }
                Console.Error.WriteLine();
                Console.Error.WriteLine("Please provide a more specific pattern.");
                Environment.Exit(1);
            }

            var targetBrew = matchingBrews[0];

            Console.WriteLine($"Running: {targetBrew.GetType().FullName}");
            Console.WriteLine($"Description: {targetBrew.Description ?? "N/A"}");
            Console.WriteLine();

            try
            {
                await targetBrew.RunAsync(source.Token);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error running {targetBrew.GetType().Name}: {ex.Message}");
                Environment.Exit(1);
            }
            break;

        case "all":
        default:
            Console.WriteLine($"Running all {brews.Count} brews in parallel...");
            Console.WriteLine();

            await Parallel.ForEachAsync(brews, source.Token, async (brew, token) =>
            {
                try 
                {
                    Console.WriteLine($"[{brew.GetType().Name}] Starting...");
                    await brew.RunAsync(token);
                    Console.WriteLine($"[{brew.GetType().Name}] Completed");
                } 
                catch (Exception ex) 
                {
                    Console.Error.WriteLine($"[{brew.GetType().Name}] Error: {ex.Message}");
                }
            });
            break;
    }
}

IHostBuilder ConfigureBuilder()
{
    return Host
        .CreateDefaultBuilder()
        .ConfigureLogging(x => x.AddConsole())
        .ConfigureServices((context, collection) =>
        {
            // Discover all IBrew implementations from referenced assemblies
            // Force load all Brew.Features assemblies from the output directory
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? Directory.GetCurrentDirectory();
            
            var featureDlls = Directory.GetFiles(assemblyDirectory, "Brew.Features.*.dll");
            var loadedAssemblies = new List<Assembly>();

            foreach (var dll in featureDlls)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    loadedAssemblies.Add(assembly);
                }
                catch
                {
                    // Skip assemblies that can't be loaded
                }
            }

            foreach (var assembly in loadedAssemblies)
            {
                foreach (var type in assembly.DefinedTypes.Where(t => typeof(IBrew).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract))
                {
                    collection.AddSingleton(typeof(IBrew), type);
                }
            }
        });
}