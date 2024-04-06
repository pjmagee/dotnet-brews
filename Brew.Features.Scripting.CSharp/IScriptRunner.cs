namespace Brew.Features.Scripting.CSharp;

public interface IScriptRunner
{
    Task RunScriptAsync();

    Task PerformOperation(double num1, double num2, string op);
}