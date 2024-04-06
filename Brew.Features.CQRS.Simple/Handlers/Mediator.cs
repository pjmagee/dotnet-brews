using Brew.Features.CQRS.Simple.Commands;
using Brew.Features.CQRS.Simple.Queries;

namespace Brew.Features.CQRS.Simple.Handlers;

using Microsoft.Extensions.DependencyInjection;

public class Mediator(IServiceProvider serviceProvider)
{
    public async Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        await handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return await handler.HandleAsync((dynamic)query, cancellationToken);
    }
}
