// Legacy interface kept for backward compatibility.
namespace Brew.Features.Builders.FluentBuilder;

public interface IStep3
{
    IComplete FinalStep(string step2Data);
}