namespace Brew.Features.Patterns.Facade;

public class ConsumingServiceAfter(FacadeService facadeService)
{
    public void ComplexOperation() => facadeService.ComplexOperation();
}