namespace Brew.Features.Mef;

public interface ICalculatorOperation
{
    public string Operation { get; }
    double Operate(double arg1, double arg2);
}