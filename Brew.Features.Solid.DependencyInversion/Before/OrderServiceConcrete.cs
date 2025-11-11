using Brew.Features.Solid.DependencyInversion.Shared;

namespace Brew.Features.Solid.DependencyInversion.Before;

/// <summary>
/// VIOLATES Dependency Inversion Principle:
/// - High-level module (OrderService) depends directly on low-level module (OrderRepository)
/// - Creates concrete dependency internally - tightly coupled
/// - Cannot be tested or swapped easily
/// </summary>
public class OrderServiceConcrete
{
    private readonly OrderRepository _orderRepository = new(new List<Order>()); // Direct dependency on concrete class
    
    public void AddOrder(Order order) => _orderRepository.Add(order);
}