using Brew.Features.Solid.SingleResponsibility.Shared;

namespace Brew.Features.Solid.SingleResponsibility.After;

public class PaymentProcessor
{
    // payment integration service provider 

    public bool Process(decimal amount, Payment payment)
    {
        return true;
    }
}