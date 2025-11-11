using MediatR;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Query to get order details
/// Queries only read data and don't change state
/// </summary>
public record GetOrderQuery(Guid OrderId) : IRequest<OrderDetails?>;

/// <summary>
/// Query to get all orders
/// </summary>
public record GetAllOrdersQuery : IRequest<List<OrderDetails>>;

/// <summary>
/// Order details DTO
/// </summary>
public record OrderDetails(
    Guid Id,
    string ProductName,
    int Quantity,
    decimal Price,
    decimal Total,
    string Status,
    DateTime CreatedAt
);
