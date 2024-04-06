using Brew.Features.Solid.SingleResponsibility.Shared;

namespace Brew.Features.Solid.SingleResponsibility.After;

public class OrderAfter(Checkout checkout)
{
    public List<OrderItem> Items { get; } = new();
    public void AddItem(OrderItem item) => Items.Add(item);

    public void RemoveItem(OrderItem item) => Items.Remove(item);
    public void Checkout(Payment payment) => checkout.Process(this, payment);
}