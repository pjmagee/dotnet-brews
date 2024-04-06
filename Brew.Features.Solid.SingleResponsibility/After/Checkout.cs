using Brew.Features.Solid.SingleResponsibility.Shared;

namespace Brew.Features.Solid.SingleResponsibility.After;

public class Checkout(Emailer emailer, PaymentProcessor paymentProcessor)
{
    public void Process(OrderAfter order, Payment payment)
    {
        // checkout handles the price calculation

        decimal price = order.Items.Sum(x => x.Price);

        // payment handles the payment process
        if (paymentProcessor.Process(price, payment))
        {
            // emailer handles building the email from the order
            emailer.SendEmail(payment.Email, order);
        }
    }
}