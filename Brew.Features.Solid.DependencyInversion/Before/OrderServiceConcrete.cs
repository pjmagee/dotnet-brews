using Brew.Features.Solid.DependencyInversion.Shared;

namespace Brew.Features.Solid.DependencyInversion.Before;

public class OrderServiceConcrete
{
    private readonly OrderRepository _orderRepository = new(new List<Order>()); // concrete implementation
    public void AddOrder(Order order) => _orderRepository.Add(order);
}