namespace Brew.Features.CQRS.Simple.Handlers;

// Assuming ICommandHandler and IQueryHandler definitions remain the same

public interface IHandlerResolver
{
    object ResolveHandler(Type handlerType);
}
