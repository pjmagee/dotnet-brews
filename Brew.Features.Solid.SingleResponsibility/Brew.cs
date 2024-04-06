using Brew.Features.Solid.SingleResponsibility.After;
using Brew.Features.Solid.SingleResponsibility.Before;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.SingleResponsibility;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<BeforeExample>().AddSingleton<OrderBefore>();
        
        services.AddSingleton<AfterExample>()
            .AddSingleton<OrderAfter>()
            .AddSingleton<Emailer>(x => new Emailer(x.GetRequiredService<ILogger<Emailer>>(), "smtp://localhost"))
            .AddSingleton<PaymentProcessor>()
            .AddSingleton<Checkout>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var beforeExample = Host.Services.GetRequiredService<BeforeExample>();
        beforeExample.Execute();
        
        var afterExample = Host.Services.GetRequiredService<AfterExample>();
        afterExample.Execute();
        
        return Task.CompletedTask;
    }
}