using Brew;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(builder => builder.ClearProviders().AddConsole());

foreach (var type in typeof(IBrew).Assembly.DefinedTypes.Where(t => typeof(IBrew).IsAssignableFrom(t) && t.IsClass))
{
    services.AddSingleton(typeof(IBrew), type);
}

await using (var provider = services.BuildServiceProvider())
{
    using (var scope = provider.CreateScope())
    {
        foreach (var brew in scope.ServiceProvider.GetRequiredService<IEnumerable<IBrew>>())
        {   
            brew.Execute();
        }
    }
}