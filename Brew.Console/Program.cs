using System.Reflection;
using Brew;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

CancellationTokenSource source = new CancellationTokenSource();

Console.CancelKeyPress += (sender, args) =>
{
    args.Cancel = true;
    source.Cancel();
};

using(var host = ConfigureBuilder().Build())
{
    await Parallel.ForEachAsync(host.Services.GetServices<IBrew>(), source.Token, async (brew, token) =>
    {
        await brew.RunAsync(token);
    });
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