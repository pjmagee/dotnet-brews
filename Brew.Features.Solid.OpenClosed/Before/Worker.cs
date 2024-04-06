namespace Brew.Features.Solid.OpenClosed.Before;

public class Worker
{
    public virtual void DoWork(string[] workItems, Strategy strategy)
    {
        // modification 1
        if (strategy == Strategy.Faster)
        {
            // do work with workItems
            Thread.Sleep(1);
        }

        // modification 2
        if (strategy == Strategy.Fast)
        {
            // do work with workItems
            Thread.Sleep(100);
        }

        // modification 3
        if (strategy == Strategy.Normal)
        {
            // do work with workItems
            Thread.Sleep(1000);
        }

        // the list goes on... now this one method is doing all kinds of work
        // maybe even now the unit tests for this one class will need to be modified
        // instead of being able to add new tests for new features
        // the maintenance of the class is going to be harder because now it has many behaviours
    }
}