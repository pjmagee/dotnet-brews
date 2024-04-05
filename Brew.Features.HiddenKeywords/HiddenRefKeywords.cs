using Microsoft.Extensions.Logging;

namespace Brew.Features.HiddenKeywords;

public class HiddenRefKeywords(ILogger<HiddenRefKeywords> logger)
{
    public void Before()
    {
        int myInteger = 10;
        TypedReference typedReference = __makeref(myInteger);

        logger.LogInformation("Value: {Unknown}", __refvalue(typedReference, int));
        logger.LogInformation("Type: {Unknown}", __reftype(typedReference));

        __refvalue(typedReference, int) = 1000;

        logger.LogInformation("{MyInteger}", myInteger);
    }

    public void After()
    {
        int myInt = 0;
        myInt = 1000;
        logger.LogInformation("{MyInt}", myInt);
    }
}