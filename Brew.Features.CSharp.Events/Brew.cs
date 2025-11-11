using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.Events;

/*
 * C# Events provide a publisher-subscriber pattern where:
 * - Publishers raise events when something happens (e.g., state change, action completion)
 * - Subscribers attach handlers to be notified when events occur
 * - Events are based on delegates (typically EventHandler or EventHandler<T>)
 * 
 * Benefits:
 * - Loose coupling: publishers don't need to know subscribers
 * - Multiple subscribers: many objects can listen to one event
 * - Standard pattern for notifications in .NET
 */
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Register publishers
        services.AddSingleton<OrderProcessor>();
        services.AddSingleton<PaymentProcessor>();
        
        // Register subscribers
        services.AddSingleton<EmailNotifier>();
        services.AddSingleton<InventoryManager>();
        services.AddSingleton<AuditLogger>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("==================== C# Events Pattern ====================");
        Logger.LogInformation("Demonstrates publisher/subscriber pattern using native C# events");
        Logger.LogInformation("Events enable decoupled communication: publishers don't know subscribers");
        Logger.LogInformation("===========================================================");
        Logger.LogInformation("");

        var orderProcessor = Host.Services.GetRequiredService<OrderProcessor>();
        var paymentProcessor = Host.Services.GetRequiredService<PaymentProcessor>();
        
        // Subscribers
        var emailNotifier = Host.Services.GetRequiredService<EmailNotifier>();
        var inventoryManager = Host.Services.GetRequiredService<InventoryManager>();
        var auditLogger = Host.Services.GetRequiredService<AuditLogger>();

        // Wire up subscriptions
        Logger.LogInformation("------------ Subscribing Handlers to Events ---------------");
        emailNotifier.SubscribeToOrders(orderProcessor);
        inventoryManager.SubscribeToOrders(orderProcessor);
        auditLogger.SubscribeToAll(orderProcessor, paymentProcessor);
        Logger.LogInformation("Subscriptions complete: EmailNotifier, InventoryManager, AuditLogger registered");
        Logger.LogInformation("");

        // Trigger events
        Logger.LogInformation("----------------- Triggering Events -----------------------");
        orderProcessor.PlaceOrder(orderId: 1001, productName: "Laptop", quantity: 2);
        Logger.LogInformation("");
        
        paymentProcessor.ProcessPayment(orderId: 1001, amount: 1999.98m);
        Logger.LogInformation("");
        
        orderProcessor.CancelOrder(orderId: 1001);
        Logger.LogInformation("");

        Logger.LogInformation("======================= BENEFITS ==========================");
        Logger.LogInformation("✓ Decoupling: OrderProcessor doesn't know about EmailNotifier/InventoryManager");
        Logger.LogInformation("✓ Extensibility: Add new subscribers without modifying publishers");
        Logger.LogInformation("✓ Multiple handlers: Many subscribers react to the same event");
        Logger.LogInformation("✓ Standard .NET pattern for asynchronous notifications");
        Logger.LogInformation("===========================================================");

        return Task.CompletedTask;
    }
}