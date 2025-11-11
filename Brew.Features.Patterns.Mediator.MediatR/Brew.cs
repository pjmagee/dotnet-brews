using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Demonstrates the Mediator Pattern using MediatR library
/// Use Case: Order management system with Commands, Queries, and Notifications
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register MediatR and all handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Brew>());
        
        // Register repository
        services.AddSingleton<OrderRepository>();
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("=== MEDIATOR PATTERN (MediatR) DEMONSTRATION ===\n");
        Logger.LogInformation("Scenario: Order management with decoupled command/query/notification handling\n");

        var mediator = Host.Services.GetRequiredService<IMediator>();

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  MEDIATOR PATTERN BENEFITS                                ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");
        
        Logger.LogInformation("✓ DECOUPLING: Components don't reference each other directly");
        Logger.LogInformation("✓ SINGLE HANDLER: Commands/Queries have exactly ONE handler");
        Logger.LogInformation("✓ MULTIPLE HANDLERS: Notifications can have MANY handlers");
        Logger.LogInformation("✓ SEPARATION: Commands (write), Queries (read), Notifications (events)");
        Logger.LogInformation("✓ TESTABILITY: Easy to test handlers in isolation\n");

        await Task.Delay(500);

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  COMMANDS - State-Changing Operations                     ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        // Create orders using commands
        Logger.LogInformation("🛒 Creating Orders...\n");

        var order1Id = await mediator.Send(new CreateOrderCommand("Laptop", 1, 1299.99m), token);
        Logger.LogInformation("");
        await Task.Delay(200);

        var order2Id = await mediator.Send(new CreateOrderCommand("Mouse", 2, 29.99m), token);
        Logger.LogInformation("");
        await Task.Delay(200);

        var order3Id = await mediator.Send(new CreateOrderCommand("Keyboard", 1, 89.99m), token);
        Logger.LogInformation("\n");
        await Task.Delay(200);

        Logger.LogInformation("Notice: Each command triggered multiple notification handlers!");
        Logger.LogInformation("- Email handler sent confirmation");
        Logger.LogInformation("- Analytics handler recorded metrics");
        Logger.LogInformation("- Inventory handler reserved stock");
        Logger.LogInformation("All handlers executed independently without coupling!\n");

        await Task.Delay(500);

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  QUERIES - Read-Only Operations                           ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("📋 Querying Orders...\n");

        // Query single order
        var order = await mediator.Send(new GetOrderQuery(order1Id), token);
        if (order != null)
        {
            Logger.LogInformation("Retrieved Order Details:");
            Logger.LogInformation("  Product: {ProductName}", order.ProductName);
            Logger.LogInformation("  Quantity: {Quantity}", order.Quantity);
            Logger.LogInformation("  Total: ${Total:N2}", order.Total);
            Logger.LogInformation("  Status: {Status}\n", order.Status);
        }

        await Task.Delay(300);

        // Query all orders
        var allOrders = await mediator.Send(new GetAllOrdersQuery(), token);
        Logger.LogInformation("\nAll Orders Summary:");
        foreach (var o in allOrders)
        {
            Logger.LogInformation("  • {ProductName} (Qty: {Quantity}) - ${Total:N2} [{Status}]",
                o.ProductName, o.Quantity, o.Total, o.Status);
        }
        Logger.LogInformation("");

        await Task.Delay(500);

        Logger.LogInformation("\n╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  STATUS UPDATES - Commands with Notifications             ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("📦 Updating Order Statuses...\n");

        await mediator.Send(new UpdateOrderStatusCommand(order1Id, "Processing"), token);
        Logger.LogInformation("");
        await Task.Delay(200);

        await mediator.Send(new UpdateOrderStatusCommand(order1Id, "Shipped"), token);
        Logger.LogInformation("");
        await Task.Delay(200);

        await mediator.Send(new UpdateOrderStatusCommand(order1Id, "Delivered"), token);
        Logger.LogInformation("\n");
        await Task.Delay(500);

        Logger.LogInformation("╔════════════════════════════════════════════════════════════╗");
        Logger.LogInformation("║  CANCELLATIONS - Commands with Side Effects               ║");
        Logger.LogInformation("╚════════════════════════════════════════════════════════════╝\n");

        Logger.LogInformation("❌ Cancelling Order...\n");

        await mediator.Send(new CancelOrderCommand(order2Id), token);
        Logger.LogInformation("\n");
        await Task.Delay(300);

        // Verify cancellation
        var finalOrders = await mediator.Send(new GetAllOrdersQuery(), token);
        Logger.LogInformation("\nFinal Orders (after cancellation):");
        foreach (var o in finalOrders)
        {
            Logger.LogInformation("  • {ProductName} - ${Total:N2} [{Status}]",
                o.ProductName, o.Total, o.Status);
        }

        Logger.LogInformation("\n" + new string('═', 60));
        Logger.LogInformation("=== KEY MEDIATOR PATTERN CONCEPTS ===");
        Logger.LogInformation(new string('═', 60));
        Logger.LogInformation("✓ REQUESTS: Commands & Queries sent through mediator");
        Logger.LogInformation("✓ HANDLERS: Process requests - ONE handler per command/query");
        Logger.LogInformation("✓ NOTIFICATIONS: Events that trigger MULTIPLE handlers");
        Logger.LogInformation("✓ PIPELINE: Can add cross-cutting concerns (logging, validation)");
        Logger.LogInformation("✓ LOOSE COUPLING: No direct dependencies between components");
        Logger.LogInformation("\nUse Cases: CQRS, event-driven systems, complex workflows, microservices");
        Logger.LogInformation("Popular Libraries: MediatR, Brighter, Wolverine");
    }
}