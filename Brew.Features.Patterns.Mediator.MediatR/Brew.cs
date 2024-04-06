using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Mediator.MediatR;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<MediatorNotifier>());
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var mediator = Host.Services.GetRequiredService<IMediator>();
        return mediator.Publish(new MediatorNotification(), CancellationToken.None);
    }
}