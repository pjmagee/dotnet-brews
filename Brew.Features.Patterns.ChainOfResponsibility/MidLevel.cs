namespace Brew.Features.Patterns.ChainOfResponsibility;

public class MidLevel : Employee
{
    public override void SignOff(Request request)
    {
        if (request.Type == RequestType.Level2)
        {
            // Handle the request
        }
        else Successor?.SignOff(request);
    }
}