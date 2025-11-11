using Brew.Features.CQRS.Simple.Models;
using Brew.Features.CQRS.Simple.Stores;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple.Commands;

public class AddProductCommandHandler(
    ProductWriteStore writeStore,
    ProductReadStore readStore,
    ILogger<AddProductCommandHandler> logger) : ICommandHandler<AddProductCommand>
{
    public Task HandleAsync(AddProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("[COMMAND] Adding product: {Name} (ID: {Id})", command.Name, command.Id);
        
        var product = new Product
        {
            Id = command.Id,
            Name = command.Name,
            Stock = command.Stock,
            Price = command.Price,
            IsActive = true
        };

        // Write to write store
        writeStore.Add(product);
        
        // Sync to read store (in real CQRS, this might be async via events)
        readStore.Sync(product);
        
        logger.LogInformation("[COMMAND] Product added successfully");
        return Task.CompletedTask;
    }
}
