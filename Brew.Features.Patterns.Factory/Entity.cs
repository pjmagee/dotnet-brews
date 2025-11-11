namespace Brew.Features.Patterns.Factory;

/// <summary>
/// Base notification interface
/// </summary>
public interface INotification
{
    string Type { get; }
    Task SendAsync(string recipient, string subject, string message);
}