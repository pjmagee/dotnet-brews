using System.Composition;

namespace Brew.Features.Mef.Operations.Add;

[Export(typeof(ICalculatorOperation))]
public class AddOperation : ICalculatorOperation
{
    public string Operation => "+";
    public double Operate(double arg1, double arg2) => arg1 + arg2;
}