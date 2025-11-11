using MediatR;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Command to create a new order
/// Commands change state and typically don't return data (or return simple confirmation)
/// </summary>
public record CreateOrderCommand(string ProductName, int Quantity, decimal Price) : IRequest<Guid>;

/// <summary>
/// Command to cancel an existing order
/// </summary>
public record CancelOrderCommand(Guid OrderId) : IRequest<bool>;

/// <summary>
/// Command to update order status
/// </summary>
public record UpdateOrderStatusCommand(Guid OrderId, string Status) : IRequest<bool>;
