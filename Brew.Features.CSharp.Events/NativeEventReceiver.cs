using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.Events;

// Subscriber: sends email notifications for orders
public class EmailNotifier(ILogger<EmailNotifier> logger)
{
    public void SubscribeToOrders(OrderProcessor orderProcessor)
    {
        orderProcessor.OrderPlaced += OnOrderPlaced;
        orderProcessor.OrderCancelled += OnOrderCancelled;
    }

    private void OnOrderPlaced(object? sender, OrderEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] EmailNotifier: Sending order confirmation email for order {OrderId}", e.OrderId);
    }

    private void OnOrderCancelled(object? sender, OrderEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] EmailNotifier: Sending cancellation email for order {OrderId}", e.OrderId);
    }
}

// Subscriber: updates inventory based on orders
public class InventoryManager(ILogger<InventoryManager> logger)
{
    public void SubscribeToOrders(OrderProcessor orderProcessor)
    {
        orderProcessor.OrderPlaced += OnOrderPlaced;
        orderProcessor.OrderCancelled += OnOrderCancelled;
    }

    private void OnOrderPlaced(object? sender, OrderEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] InventoryManager: Reducing stock for {Product} by {Quantity}",
            e.ProductName, e.Quantity);
    }

    private void OnOrderCancelled(object? sender, OrderEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] InventoryManager: Restoring stock for order {OrderId}", e.OrderId);
    }
}

// Subscriber: logs all activity for auditing
public class AuditLogger(ILogger<AuditLogger> logger)
{
    public void SubscribeToAll(OrderProcessor orderProcessor, PaymentProcessor paymentProcessor)
    {
        orderProcessor.OrderPlaced += OnOrderPlaced;
        orderProcessor.OrderCancelled += OnOrderCancelled;
        paymentProcessor.PaymentProcessed += OnPaymentProcessed;
    }

    private void OnOrderPlaced(object? sender, OrderEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] AuditLogger: AUDIT - Order {OrderId} placed", e.OrderId);
    }

    private void OnOrderCancelled(object? sender, OrderEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] AuditLogger: AUDIT - Order {OrderId} cancelled", e.OrderId);
    }

    private void OnPaymentProcessed(object? sender, PaymentEventArgs e)
    {
        logger.LogInformation("  [SUBSCRIBER] AuditLogger: AUDIT - Payment ${Amount:F2} processed for order {OrderId}",
            e.Amount, e.OrderId);
    }
}
