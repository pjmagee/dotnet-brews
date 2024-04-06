namespace Brew.Features.Patterns.Facade;

public class FacadeService(ComplexServiceC serviceC, ComplexServiceB serviceB, ComplexServiceA serviceA)
{
    public void ComplexOperation()
    {
        serviceA.ComplexOperation();
        serviceB.ComplexOperation();
        serviceC.ComplexOperation();
    }
}