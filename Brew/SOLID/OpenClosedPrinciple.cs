using System;
using System.Linq;
using System.Threading;

namespace Brew.SOLID;

/// <summary>
/// Open for extension, closed for modification
/// </summary>
public class OpenClosedPrinciple : IBrew
{
    public enum Strategy
    {
        Slower,
        Slow,
        Normal,
        Fast,
        Faster
    }

    #region Before 

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

    public void Before()
    {
        Worker worker = new Worker();

        foreach (var strategy in Enum.GetValues(typeof(Strategy)).OfType<Strategy>())
        {
            // The same class is doing all the work
            // its breaching the single responsibility principle, which is what the open closed principle helps solve
            // constantly modifying is going to add complexity to the individual class
            worker.DoWork(new[] { "item 1", "item 2", "item 3" }, strategy);
        }
    }

    #endregion

    #region After

    public void After()
    {
        var items = new[] { "item 1", "item 2", "item 3" };

        FastWorker fastWorker = new FastWorker();
        fastWorker.DoWork(items);

        SlowWorker slowWorker = new SlowWorker();
        slowWorker.DoWork(items);
    }

    public class OpenClosedWorker : Worker
    {
        public virtual void DoWork(string[] workItems)
        {
            // single responsibility 
        }
    }

    public class FastWorker : OpenClosedWorker
    {
        public override void DoWork(string[] workItems)
        {
            // do things fast
        }
    }

    public class SlowWorker : OpenClosedWorker
    {
        public override void DoWork(string[] workItems)
        {
            // do things slow
        }
    }


    #endregion

}
