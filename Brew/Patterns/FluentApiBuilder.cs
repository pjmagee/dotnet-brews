namespace Brew.Patterns;

public class FluentApiBuilder : IBrew
{
    public void Before()
    {

    }

    public void After()
    {
        new FluentBuilder()
                .DoStepOne("some step 1 data")
                .WithStep2("some step 2 data")
                .FinalStep("some step 3 data")
                .Execute();
    }

    public class FluentBuilder : IStep1, IStep2, IStep3, IComplete
    {
        private string step1;
        private string step2;
        private string step3;

        public IStep2 DoStepOne(string step1Data)
        {
            step1 = step1Data;
            return this;
        }

        IStep3 IStep2.WithStep2(string step2Data)
        {
            step2 = step2Data;
            return this;
        }

        IComplete IStep3.FinalStep(string step3Data)
        {
            step3 = step3Data;
            return this;
        }

        void IComplete.Execute()
        {
            // use previous set information
        }
    }

    public interface IStep1
    {
        IStep2 DoStepOne(string step1Data);
    }

    public interface IStep2
    {
        IStep3 WithStep2(string step2Data);
    }

    public interface IStep3
    {
        IComplete FinalStep(string step2Data);
    }

    public interface IComplete
    {
        void Execute();
    }

}