using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Brew.Tests;

public class BrewExamples
{
    private readonly ITestOutputHelper helper;

    public BrewExamples(ITestOutputHelper helper)
    {
        this.helper = helper;
    }

    [Fact]
    public async Task Test()
    {
        var services = new ServiceCollection();
        services.AddLogging((builder) => builder.AddXUnit(helper));

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
                    helper.WriteLine("Start: " + brew.GetType().Name);
                    brew.Execute();
                    helper.WriteLine("End: " + brew.GetType().Name);
                }
            }
        }
    }
}