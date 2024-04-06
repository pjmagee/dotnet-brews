namespace Brew.Features.Patterns.ChainOfResponsibility;

public class EntryLevel : Employee
{
    public override void SignOff(Request request)
    {
        if (request.Type == RequestType.Level1)
        {
            // Handle the request
        }
        else Successor?.SignOff(request);
    }
}