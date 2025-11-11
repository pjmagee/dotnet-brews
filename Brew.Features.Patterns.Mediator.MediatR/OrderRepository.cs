using System.Collections.Concurrent;

namespace Brew.Features.Patterns.Mediator.MediatR;

/// <summary>
/// Simple in-memory repository for demo purposes
/// </summary>
public class OrderRepository
{
    private readonly ConcurrentDictionary<Guid, OrderDetails> _orders = new();

    public void Add(Guid id, OrderDetails order)
    {
        _orders[id] = order;
    }

    public bool TryGet(Guid id, out OrderDetails? order)
    {
        return _orders.TryGetValue(id, out order);
    }

    public void Update(Guid id, OrderDetails order)
    {
        _orders[id] = order;
    }

    public void Remove(Guid id)
    {
        _orders.TryRemove(id, out _);
    }

    public List<OrderDetails> GetAll()
    {
        return _orders.Values.ToList();
    }
}
