using Brew.Features.CQRS.Simple.Commands;
using Brew.Features.CQRS.Simple.Handlers;
using Brew.Features.CQRS.Simple.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<Mediator>();
        services.AddSingleton<List<string>>(); // Simulating a shared in-memory data store
        services.AddTransient<ICommandHandler<SubmitMessageCommand>, SubmitMessageCommandHandler>();
        services.AddTransient<ICommandHandler<PrintMessageCommand>, PrintMessageCommandHandler>();
        services.AddTransient<IQueryHandler<GetDataQuery, IEnumerable<string>>, GetDataQueryHandler>();
        services.AddSingleton<IServiceProvider>(provider => provider);
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        var mediator = Host.Services.GetRequiredService<Mediator>();

        await mediator.DispatchAsync(new PrintMessageCommand("This is a simple command to print, not store, a message"), token);
            
        await mediator.DispatchAsync(new SubmitMessageCommand("Hello, World! This is a simple command to store a message"), token);

        foreach  (var item in await mediator.DispatchAsync(new GetDataQuery(), token))
        {
            Logger.LogInformation("Message: {Message}", item);
        }
    }
}