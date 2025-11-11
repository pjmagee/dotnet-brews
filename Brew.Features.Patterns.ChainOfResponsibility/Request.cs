namespace Brew.Features.Patterns.ChainOfResponsibility;

/// <summary>
/// Request object containing purchase information
/// </summary>
public class Request
{
    public RequestType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public bool IsApproved { get; set; }
}