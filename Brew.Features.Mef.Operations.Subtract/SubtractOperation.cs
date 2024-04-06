using System.Composition;

namespace Brew.Features.Mef.Operations.Subtract;

[Export(typeof(ICalculatorOperation))]
public class SubtractOperation : ICalculatorOperation
{
    public string Operation => "-";
    public double Operate(double arg1, double arg2) => arg1 - arg2;
}