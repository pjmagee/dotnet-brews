using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Adaptor;

public class Adaptors : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        var adaptee = new Adaptee();
        var result = "Hello " + adaptee.Hello();
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        Adaptor adaptor = new Adaptor(new Adaptee());
        adaptor.Hello();
        adaptor.GetEntity();
        
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
