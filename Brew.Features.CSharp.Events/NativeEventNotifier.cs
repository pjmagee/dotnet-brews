using Microsoft.Extensions.Logging;

namespace Brew.Features.CSharp.Events;

// Event arguments for order events
public class OrderEventArgs(int orderId, string productName, int quantity) : EventArgs
{
    public int OrderId { get; } = orderId;
    public string ProductName { get; } = productName;
    public int Quantity { get; } = quantity;
}

// Event arguments for payment events
public class PaymentEventArgs(int orderId, decimal amount) : EventArgs
{
    public int OrderId { get; } = orderId;
    public decimal Amount { get; } = amount;
}

// Publisher: raises events when orders are processed
public class OrderProcessor(ILogger<OrderProcessor> logger)
{
    // Events using custom EventArgs
    public event EventHandler<OrderEventArgs>? OrderPlaced;
    public event EventHandler<OrderEventArgs>? OrderCancelled;

    public void PlaceOrder(int orderId, string productName, int quantity)
    {
        logger.LogInformation("[PUBLISHER] OrderProcessor: Placing order {OrderId} for {Quantity}x {Product}",
            orderId, quantity, productName);
        
        // Raise event - all subscribers will be notified
        OrderPlaced?.Invoke(this, new OrderEventArgs(orderId, productName, quantity));
    }

    public void CancelOrder(int orderId)
    {
        logger.LogInformation("[PUBLISHER] OrderProcessor: Cancelling order {OrderId}", orderId);
        OrderCancelled?.Invoke(this, new OrderEventArgs(orderId, "N/A", 0));
    }
}

// Publisher: raises events when payments are processed
public class PaymentProcessor(ILogger<PaymentProcessor> logger)
{
    public event EventHandler<PaymentEventArgs>? PaymentProcessed;

    public void ProcessPayment(int orderId, decimal amount)
    {
        logger.LogInformation("[PUBLISHER] PaymentProcessor: Processing payment for order {OrderId}: ${Amount:F2}",
            orderId, amount);
        
        PaymentProcessed?.Invoke(this, new PaymentEventArgs(orderId, amount));
    }
}
