using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Facade;

/*
 * Facades can be used to encapsulate sharable or repeatable logic
 * This keeps the layers above abstract and simpler
 */
public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        // Each time services are needed they need to be individually injected. 
        ConsumingServiceBefore consumingService = new ConsumingServiceBefore(new ComplexServiceC(), new ComplexServiceB(), new ComplexServiceA());
        consumingService.ComplexOperation();
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        // Facade abstracts the behaviour or wraps this behaviour for simplified consumption and flow
        var facade = new FacadeService(new ComplexServiceC(), new ComplexServiceB(), new ComplexServiceA());

        // Pass facade into any consuming service
        var consumingService = new ConsumingServiceAfter(facade);
        consumingService.ComplexOperation();
        
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