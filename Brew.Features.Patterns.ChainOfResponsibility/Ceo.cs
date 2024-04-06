namespace Brew.Features.Patterns.ChainOfResponsibility;

public class Ceo : Employee
{
    public override void SignOff(Request request)
    {
        if (request.Type == RequestType.Level3)
        {
            // Handle the request
        }
        else Successor?.SignOff(request);
    }
}