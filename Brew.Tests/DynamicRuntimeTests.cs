using System.Threading.Tasks;
using Brew.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Brew.Tests;

public class DynamicRuntimeTests
{
    private readonly ITestOutputHelper helper;

    public DynamicRuntimeTests(ITestOutputHelper helper)
    {
        this.helper = helper;
    }

    [Fact]
    public async Task Test()
    {
        using (var provider = new ServiceCollection()
                   .AddLogging((builder) => builder.AddXUnit(helper))
                   .AddSingleton<DynamicRuntime>().BuildServiceProvider().CreateAsyncScope())
        {
            var drt = provider.ServiceProvider.GetRequiredService<DynamicRuntime>();

            drt.Execute();
        }
    }
}