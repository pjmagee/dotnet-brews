namespace Brew.Patterns;

/*
 * Facades can be used to encapsulate sharable or repeatable logic
 * This keeps the layers above abstract and simpler
 */
public class Facade : IBrew
{
    public void Before()
    {
        // Each time services are needed they need to be individually injected. 
        ConsumingServiceBefore consumingService = new ConsumingServiceBefore(new ComplexServiceC(), new ComplexServiceB(), new ComplexServiceA());

        consumingService.ComplexOperation();
    }

    public void After()
    {
        // Facade abstracts the behaviour or wraps this behaviour for simplified consumption and flow
        var facade = new FacadeService(new ComplexServiceC(), new ComplexServiceB(), new ComplexServiceA());

        // Pass facade into any consuming service
        var consumingService = new ConsumingServiceAfter(facade);

        consumingService.ComplexOperation();
    }


    #region Complex Services

    public class ConsumingServiceBefore
    {
        private ComplexServiceC serviceC;
        private ComplexServiceB serviceB;
        private ComplexServiceA serviceA;

        public ConsumingServiceBefore(ComplexServiceC serviceC, ComplexServiceB serviceB, ComplexServiceA serviceA)
        {
            this.serviceC = serviceC;
            this.serviceB = serviceB;
            this.serviceA = serviceA;
        }

        public void ComplexOperation()
        {
            // ConsumingServiceBefore is having to call and manage all the injected services
            serviceA.ComplexOperation();
            serviceB.ComplexOperation();
            serviceC.ComplexOperation();
        }
    }

    public class ComplexServiceA
    {
        public void ComplexOperation() { }
    }

    public class ComplexServiceC
    {
        public void ComplexOperation() { }
    }

    public class ComplexServiceB
    {
        public void ComplexOperation() { }
    }

    #endregion

    public class ConsumingServiceAfter
    {
        FacadeService facadeService;

        public ConsumingServiceAfter(FacadeService facadeService)
        {
            this.facadeService = facadeService;
        }

        public void ComplexOperation() => facadeService.ComplexOperation();
    }

    public class FacadeService
    {
        private readonly ComplexServiceC _serviceC;
        private readonly ComplexServiceB _serviceB;
        private readonly ComplexServiceA _serviceA;

        public FacadeService(ComplexServiceC serviceC, ComplexServiceB serviceB, ComplexServiceA serviceA)
        {
            _serviceC = serviceC;
            _serviceB = serviceB;
            _serviceA = serviceA;
        }

        public void ComplexOperation()
        {
            _serviceA.ComplexOperation();
            _serviceB.ComplexOperation();
            _serviceC.ComplexOperation();
        }
    }
}
