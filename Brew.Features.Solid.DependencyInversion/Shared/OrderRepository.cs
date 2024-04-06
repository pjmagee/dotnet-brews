namespace Brew.Features.Solid.DependencyInversion.Shared;

public class OrderRepository(ICollection<Order> orders)
{
    public virtual void Add(Order order) => orders.Add(order);

    public virtual void Remove(Order order) => orders.Remove(order);

    public virtual Order GetById(int id) => orders.First(x => x.Id == id);
}
