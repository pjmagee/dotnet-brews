using Brew.Features.Solid.SingleResponsibility.Shared;

namespace Brew.Features.Solid.SingleResponsibility.After;

public class AfterExample(OrderAfter after)
{
    
    public void Execute()
    {

        after.AddItem(new OrderItem() { Price = 2_0, ProductCode = 1, ProductName = "Bread" });
        after.AddItem(new OrderItem() { Price = 2_50, ProductCode = 1, ProductName = "Milk" });

        after.Checkout(new Payment()
        {
            Details = new Dictionary<string, string>()
            {
                { "number" , "03391203193110" },
                { "CSV" , "XXX" },
                { "Expire" , "2024" },
                { "Address" , "Billing Address" }
            }
        });
    }
}
