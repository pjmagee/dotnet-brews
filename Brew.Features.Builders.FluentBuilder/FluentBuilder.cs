using Microsoft.Extensions.Logging;

namespace Brew.Features.Builders.FluentBuilder;

public class FluentBuilder(ILogger<FluentBuilder> logger) : IStep1, IStep2, IStep3, IComplete
{
    private string step1;
    private string step2;
    private string step3;

    public IStep2 DoStepOne(string step1Data)
    {
        step1 = step1Data;
        logger.LogInformation("Step 1 completed");
        return this;
    }

    IStep3 IStep2.WithStep2(string step2Data)
    {
        step2 = step2Data;
        logger.LogInformation("Step 2 completed");
        return this;
    }

    IComplete IStep3.FinalStep(string step3Data)
    {
        step3 = step3Data;
        logger.LogInformation("Step 3 completed");
        return this;
    }

    void IComplete.Execute()
    {
        logger.LogInformation("Executing steps");
    }
}