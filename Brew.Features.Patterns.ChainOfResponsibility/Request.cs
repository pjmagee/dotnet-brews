namespace Brew.Features.Patterns.ChainOfResponsibility;

public class Request
{
    public RequestType Type { get; set; }
    public string Details { get; set; }
    public string SignedOffBy { get; set; }
}