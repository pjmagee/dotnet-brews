namespace Brew.Features.Patterns.Facade;

public class ConsumingServiceBefore(ComplexServiceC serviceC, ComplexServiceB serviceB, ComplexServiceA serviceA)
{
    public void ComplexOperation()
    {
        // ConsumingServiceBefore is having to call and manage all the injected services
        serviceA.ComplexOperation();
        serviceB.ComplexOperation();
        serviceC.ComplexOperation();
    }
}