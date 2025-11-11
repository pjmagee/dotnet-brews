namespace Brew.Features.Solid.DependencyInversion.Shared;

/// <summary>
/// Low-level module that implements the abstraction (IOrderRepository)
/// This is the "detail" that depends on the abstraction
/// </summary>
public class OrderRepository(ICollection<Order> orders) : IOrderRepository
{
    public void Add(Order order) => orders.Add(order);

    public void Remove(Order order) => orders.Remove(order);

    public Order GetById(int id) => orders.First(x => x.Id == id);
}
