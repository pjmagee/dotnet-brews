using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Factory;

public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        BeforeConsumer consumer = new BeforeConsumer();
        consumer.DoSomething();
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        AfterConsumer consumer = new AfterConsumer(new EntityFactory());
        consumer.DoSomething();
        return Task.CompletedTask;
    }


    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}