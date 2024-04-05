using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.MediatR;

public class Brew : IBrew
{
    public async Task Execute()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, collection) =>
            {
                collection.AddMediatR(x =>
                {
                    x.RegisterServicesFromAssemblyContaining<MediatorNotifier>();
                });
            })
            .ConfigureLogging(x => x.AddConsole());

        using (var host = hostBuilder.Build())
        {
            var mediator = host.Services.GetRequiredService<IMediator>();
            await mediator.Publish(new MediatorNotification(), CancellationToken.None);
        }
    }
}