namespace Brew.Features.Patterns.ChainOfResponsibility;

public abstract class Employee
{
    protected Employee? Successor;

    public void SetSuccessor(Employee successor)
    {
        Successor = successor;
    }

    public abstract void SignOff(Request request);
}