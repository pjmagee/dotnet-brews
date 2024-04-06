using Brew.Features.Solid.DependencyInversion.Shared;

namespace Brew.Features.Solid.DependencyInversion.After;


public class OrderServiceFlexible(OrderRepository orderRepository)
{
    public void AddOrder(Order order) => orderRepository.Add(order);
}