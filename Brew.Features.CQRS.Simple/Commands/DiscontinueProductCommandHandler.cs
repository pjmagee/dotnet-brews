using Brew.Features.CQRS.Simple.Stores;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple.Commands;

public class DiscontinueProductCommandHandler(
    ProductWriteStore writeStore,
    ProductReadStore readStore,
    ILogger<DiscontinueProductCommandHandler> logger) : ICommandHandler<DiscontinueProductCommand>
{
    public Task HandleAsync(DiscontinueProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("[COMMAND] Discontinuing product {Id}", command.ProductId);

        writeStore.Update(command.ProductId, product =>
        {
            product.IsActive = false;
            logger.LogInformation("[COMMAND] Product {Name} is now discontinued", product.Name);
            
            // Sync to read store
            readStore.Sync(product);
        });

        return Task.CompletedTask;
    }
}
