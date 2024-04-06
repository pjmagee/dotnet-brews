using Brew.Features.Solid.SingleResponsibility.Shared;

namespace Brew.Features.Solid.SingleResponsibility.Before;

public class BeforeExample(OrderBefore before)
{
    public void Execute()
    {
        before.AddItem(new OrderItem() { Price = 2_0, ProductCode = 1, ProductName = "Bread " });
        before.AddItem(new OrderItem() { Price = 2_0, ProductCode = 1, ProductName = "Bread " });

        before.Checkout(new Payment()
        {
            Details = new Dictionary<string, string>()
            {
                { "number", "03391203193110" },
                { "CSV", "XXX" },
                { "Expire", "2024" },
                { "Address", "Billing Address" }
            }
        });
    }
}