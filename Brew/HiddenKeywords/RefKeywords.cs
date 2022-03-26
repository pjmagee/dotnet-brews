using System;
using Microsoft.Extensions.Logging;

namespace Brew.HiddenKeywords;

public class HiddenRefKeywords : IBrew
{
    private readonly ILogger<HiddenRefKeywords> logger;

    public HiddenRefKeywords(ILogger<HiddenRefKeywords> logger)
    {
        this.logger = logger;
    }

    public void Before()
    {
        int myInteger = 10;
        TypedReference typedReference = __makeref(myInteger);

        logger.LogInformation($"Value: {__refvalue(typedReference, int)}");
        logger.LogInformation($"Type: {__reftype(typedReference)}");

        __refvalue(typedReference, int) = 1000;

        logger.LogInformation($"{myInteger}");
    }

    public void After()
    {
        int myInt = 0;
        myInt = 1000;
        logger.LogInformation($"{myInt}");
    }
}
