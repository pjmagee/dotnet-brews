using System.Reflection;
using Brew;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using(var host = ConfigureBuilder().Build())
{
    using(var scope = host.Services.CreateScope())
    {
        var brews = scope.ServiceProvider.GetServices<IBrew>();

        foreach (var brew in brews)
        {
            brew.Execute();
        }
    }
}

IHostBuilder ConfigureBuilder()
{
    var builder = Host
        .CreateDefaultBuilder()
        .ConfigureLogging(x => x.AddConsole());

    builder.ConfigureServices((context, collection) =>
    {
        var dlls = Directory.GetFiles(Directory.GetCurrentDirectory(), "Brew.Features.*.dll");
        
        foreach(var dll in dlls)
        {
            var assembly = Assembly.LoadFrom(dll);
            
            foreach (var type in assembly.DefinedTypes.Where(t => typeof(IBrew).IsAssignableFrom(t) && t.IsClass))
            {
                collection.AddSingleton(typeof(IBrew), type);
            }
        }
    });

    return builder;
}