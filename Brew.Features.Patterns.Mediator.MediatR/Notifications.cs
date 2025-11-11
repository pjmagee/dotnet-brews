using MediatR;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Notification published when an order is created
/// Notifications can have multiple handlers
/// </summary>
public record OrderCreatedNotification(Guid OrderId, string ProductName, decimal Total) : INotification;

/// <summary>
/// Notification published when an order is cancelled
/// </summary>
public record OrderCancelledNotification(Guid OrderId) : INotification;

/// <summary>
/// Notification published when order status changes
/// </summary>
public record OrderStatusChangedNotification(Guid OrderId, string OldStatus, string NewStatus) : INotification;
