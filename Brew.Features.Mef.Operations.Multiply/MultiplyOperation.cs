using System.Composition;

namespace Brew.Features.Mef.Operations.Multiply;

[Export(typeof(ICalculatorOperation))]
public class MultiplyOperation : ICalculatorOperation
{
    public string Operation => "*";
    public double Operate(double arg1, double arg2) => arg1 * arg2;
}