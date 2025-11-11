using Brew.Features.CQRS.Simple.Stores;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple.Commands;

public class UpdateStockCommandHandler(
    ProductWriteStore writeStore,
    ProductReadStore readStore,
    ILogger<UpdateStockCommandHandler> logger) : ICommandHandler<UpdateStockCommand>
{
    public Task HandleAsync(UpdateStockCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("[COMMAND] Updating stock for product {Id}: {Change:+#;-#;0}",
            command.ProductId, command.QuantityChange);

        writeStore.Update(command.ProductId, product =>
        {
            product.Stock += command.QuantityChange;
            logger.LogInformation("[COMMAND] New stock level: {Stock}", product.Stock);
            
            // Sync to read store
            readStore.Sync(product);
        });

        return Task.CompletedTask;
    }
}
