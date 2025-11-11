using Brew.Features.Solid.DependencyInversion.Shared;

namespace Brew.Features.Solid.DependencyInversion.After;

/// <summary>
/// FOLLOWS Dependency Inversion Principle:
/// - High-level module (OrderService) depends on abstraction (IOrderRepository)
/// - Does NOT depend on concrete implementation (OrderRepository)
/// - Both high-level and low-level modules depend on the same abstraction
/// - Easy to test (can inject mock), swap implementations, extend
/// </summary>
public class OrderServiceFlexible(IOrderRepository orderRepository)
{
    public void AddOrder(Order order) => orderRepository.Add(order);
}