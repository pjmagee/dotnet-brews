// Legacy interface kept for backward compatibility; directs users to new fluent report builder.
namespace Brew.Features.Builders.FluentBuilder;

public interface IStep1
{
    IStep2 DoStepOne(string step1Data);
}