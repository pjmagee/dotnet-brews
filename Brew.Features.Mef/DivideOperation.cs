using System.Composition;

namespace Brew.Features.Mef;

[Export(typeof(ICalculatorOperation))]
public class DivideOperation : ICalculatorOperation
{
    public string Operation => "/";
    public double Operate(double arg1, double arg2) => arg1 / arg2;
}