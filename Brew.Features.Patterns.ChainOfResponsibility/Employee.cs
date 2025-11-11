using Microsoft.Extensions.Logging;

namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// Base handler in the chain of responsibility
/// </summary>
public abstract class Employee
{
    protected Employee? Successor;
    protected readonly ILogger Logger;
    public string Title { get; }

    protected Employee(ILogger logger, string title)
    {
        Logger = logger;
        Title = title;
    }

    /// <summary>
    /// Sets the next handler in the chain
    /// </summary>
    public void SetSuccessor(Employee successor)
    {
        Successor = successor;
        Logger.LogInformation("[Chain Setup] {Title} will forward unhandled requests to {SuccessorTitle}", 
            Title, successor.Title);
    }

    /// <summary>
    /// Each handler decides whether to process the request or pass it up the chain
    /// </summary>
    public abstract void ProcessRequest(Request request);
}