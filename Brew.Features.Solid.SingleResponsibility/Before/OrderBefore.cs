using Brew.Features.Solid.SingleResponsibility.Shared;

namespace Brew.Features.Solid.SingleResponsibility.Before;

public class OrderBefore
{
    private List<OrderItem> Items { get; } = new();

    public void AddItem(OrderItem item)
    {
        // add an item to the order
        DisplayCart();
    }

    public void RemoveItem(OrderItem item)
    {
        // find and remove an item from the order
        DisplayCart();
    }

    public void DisplayCart()
    {
        // Product Name x Quantity = Sum of Price
        // Display to the user
    }

    public void Checkout(Payment details)
    {
        DisplayCart();

        if (Payment(details))
            SendInvoice(details);
    }

    bool Payment(Payment details)
    {
        // charge card
        // ensure success
        return default;
    }

    void SendInvoice(Payment details)
    {
        // setup invoice object
        // setup smtp client
        // create template
        // configure email body, subject, to, from
        // send email
    }
}