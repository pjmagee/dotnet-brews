using MediatR;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Email notification handler - sends email when order is created
/// Notifications can have MULTIPLE handlers
/// </summary>
public class EmailNotificationHandler : 
    INotificationHandler<OrderCreatedNotification>,
    INotificationHandler<OrderCancelledNotification>,
    INotificationHandler<OrderStatusChangedNotification>
{
    private readonly ILogger<EmailNotificationHandler> _logger;

    public EmailNotificationHandler(ILogger<EmailNotificationHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Email Handler] Sending order confirmation email");
        _logger.LogInformation("  To: customer@example.com");
        _logger.LogInformation("  Subject: Order Confirmation - {ProductName}", notification.ProductName);
        _logger.LogInformation("  Body: Your order #{OrderId} has been placed. Total: ${Total:N2}", 
            notification.OrderId, notification.Total);
        await Task.Delay(50); // Simulate sending
        _logger.LogInformation("  ✓ Email sent successfully");
    }

    public async Task Handle(OrderCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Email Handler] Sending cancellation email");
        _logger.LogInformation("  Subject: Order Cancelled - #{OrderId}", notification.OrderId);
        await Task.Delay(50);
        _logger.LogInformation("  ✓ Email sent successfully");
    }

    public async Task Handle(OrderStatusChangedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Email Handler] Sending status update email");
        _logger.LogInformation("  Subject: Order Status Update - #{OrderId}", notification.OrderId);
        _logger.LogInformation("  Status: {OldStatus} → {NewStatus}", notification.OldStatus, notification.NewStatus);
        await Task.Delay(50);
        _logger.LogInformation("  ✓ Email sent successfully");
    }
}

/// <summary>
/// Analytics handler - tracks order metrics
/// Independent handler that can be added/removed without affecting others
/// </summary>
public class AnalyticsNotificationHandler : 
    INotificationHandler<OrderCreatedNotification>,
    INotificationHandler<OrderCancelledNotification>
{
    private readonly ILogger<AnalyticsNotificationHandler> _logger;

    public AnalyticsNotificationHandler(ILogger<AnalyticsNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Analytics Handler] Recording order creation metric");
        _logger.LogInformation("  Event: order_created");
        _logger.LogInformation("  Product: {ProductName}, Revenue: ${Total:N2}", notification.ProductName, notification.Total);
        _logger.LogInformation("  ✓ Metric recorded in analytics system");
        return Task.CompletedTask;
    }

    public Task Handle(OrderCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Analytics Handler] Recording order cancellation metric");
        _logger.LogInformation("  Event: order_cancelled");
        _logger.LogInformation("  ✓ Metric recorded in analytics system");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Inventory handler - updates stock levels
/// Shows how different concerns are separated
/// </summary>
public class InventoryNotificationHandler : INotificationHandler<OrderCreatedNotification>
{
    private readonly ILogger<InventoryNotificationHandler> _logger;

    public InventoryNotificationHandler(ILogger<InventoryNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Inventory Handler] Updating inventory");
        _logger.LogInformation("  Product: {ProductName}", notification.ProductName);
        _logger.LogInformation("  Reserving stock...");
        _logger.LogInformation("  ✓ Inventory updated successfully");
        return Task.CompletedTask;
    }
}
