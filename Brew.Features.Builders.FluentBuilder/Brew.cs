using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Builders.FluentBuilder;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddTransient<FluentBuilder>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Host.Services.GetRequiredService<FluentBuilder>()
            .DoStepOne("Step 1")
            .WithStep2("Step 2")
            .FinalStep("Step 3")
            .Execute();
        
        return Task.CompletedTask;
    }
}