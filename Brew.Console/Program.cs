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
                Console.Error.WriteLine("Usage: Brew.Console run <brew-type-name>");
                Environment.Exit(1);
            }

            var targetBrew = brews.FirstOrDefault(b => 
                b.GetType().FullName?.Contains(brewName, StringComparison.OrdinalIgnoreCase) == true ||
                b.GetType().Name.Equals(brewName, StringComparison.OrdinalIgnoreCase));

            if (targetBrew == null)
            {
                Console.Error.WriteLine($"Error: Brew '{brewName}' not found");
                Console.Error.WriteLine("Use 'list' command to see available brews");
                Environment.Exit(1);
            }

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
            var dlls = Directory.GetFiles(Directory.GetCurrentDirectory(), "Brew.Features.*.dll");

            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFrom(dll);

                foreach (var type in assembly.DefinedTypes.Where(t => typeof(IBrew).IsAssignableFrom(t) && t.IsClass))
                {
                    collection.AddSingleton(typeof(IBrew), type);
                }
            }
        });
}