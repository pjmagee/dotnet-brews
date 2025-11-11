using Brew.Features.CQRS.Simple.Commands;
using Brew.Features.CQRS.Simple.Handlers;
using Brew.Features.CQRS.Simple.Models;
using Brew.Features.CQRS.Simple.Queries;
using Brew.Features.CQRS.Simple.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CQRS.Simple;

/*
 * CQRS (Command Query Responsibility Segregation) separates write operations (Commands)
 * from read operations (Queries), enabling different models and optimization strategies.
 * 
 * Commands: Change state, no return value (void/Task)
 * Queries: Return data, never modify state
 * 
 * Benefits:
 * - Independent scaling of read/write workloads
 * - Optimized data models for each operation type (normalized write, denormalized read)
 * - Simplified reasoning: commands change things, queries fetch things
 * - Easier audit trails and event sourcing integration
 */
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Mediator for dispatching commands and queries
        services.AddSingleton<Mediator>();
        
        // Write model: normalized Product storage
        services.AddSingleton<ProductWriteStore>();
        
        // Read model: denormalized ProductSummary view (could be a separate DB, cache, etc.)
        services.AddSingleton<ProductReadStore>();
        
        // Command handlers (modify state)
        services.AddTransient<ICommandHandler<AddProductCommand>, AddProductCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateStockCommand>, UpdateStockCommandHandler>();
        services.AddTransient<ICommandHandler<DiscontinueProductCommand>, DiscontinueProductCommandHandler>();
        
        // Query handlers (read-only)
        services.AddTransient<IQueryHandler<GetProductSummariesQuery, IEnumerable<ProductSummary>>, GetProductSummariesQueryHandler>();
        services.AddTransient<IQueryHandler<GetLowStockProductsQuery, IEnumerable<ProductSummary>>, GetLowStockProductsQueryHandler>();
        
        services.AddSingleton<IServiceProvider>(provider => provider);
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("==================== CQRS Pattern Demonstration ====================");
        Logger.LogInformation("CQRS separates Commands (writes) from Queries (reads), allowing:");
        Logger.LogInformation("  - Independent optimization of write vs read models");
        Logger.LogInformation("  - Different data stores or schemas for commands and queries");
        Logger.LogInformation("  - Simplified logic: commands mutate, queries fetch");
        Logger.LogInformation("=====================================================================");

        var mediator = Host.Services.GetRequiredService<Mediator>();

        // Execute Commands (write operations - modify state)
        Logger.LogInformation("");
        Logger.LogInformation("--------------- EXECUTING COMMANDS (Write Operations) ---------------");
        
        await mediator.DispatchAsync(new AddProductCommand(1, "Laptop", 10, 999.99m), token);
        await mediator.DispatchAsync(new AddProductCommand(2, "Mouse", 50, 29.99m), token);
        await mediator.DispatchAsync(new AddProductCommand(3, "Keyboard", 5, 79.99m), token);
        
        await mediator.DispatchAsync(new UpdateStockCommand(1, -3), token); // Sold 3 laptops
        await mediator.DispatchAsync(new UpdateStockCommand(2, 25), token);  // Restocked 25 mice
        
        await mediator.DispatchAsync(new DiscontinueProductCommand(3), token); // Discontinue keyboard

        // Execute Queries (read operations - fetch data)
        Logger.LogInformation("");
        Logger.LogInformation("--------------- EXECUTING QUERIES (Read Operations) ----------------");
        
        var allProducts = await mediator.DispatchAsync(new GetProductSummariesQuery(), token);
        Logger.LogInformation("All Product Summaries:");
        foreach (var product in allProducts)
        {
            Logger.LogInformation("  [{Id}] {Name} - Stock: {Stock}, Price: ${Price:F2}, Active: {Active}",
                product.Id, product.Name, product.Stock, product.Price, product.IsActive);
        }

        Logger.LogInformation("");
        var lowStock = await mediator.DispatchAsync(new GetLowStockProductsQuery(10), token);
        Logger.LogInformation("Low Stock Products (threshold: 10):");
        foreach (var product in lowStock)
        {
            Logger.LogInformation("  [{Id}] {Name} - Stock: {Stock} (NEEDS REORDER)",
                product.Id, product.Name, product.Stock);
        }

        Logger.LogInformation("");
        Logger.LogInformation("=========================== BENEFITS ================================");
        Logger.LogInformation("✓ Write model optimized for validation & consistency");
        Logger.LogInformation("✓ Read model optimized for query performance (denormalized)");
        Logger.LogInformation("✓ Clear separation: commands = intent to change, queries = fetch");
        Logger.LogInformation("✓ Scalability: read/write stores can be independent (different DBs)");
        Logger.LogInformation("=====================================================================");
    }
}