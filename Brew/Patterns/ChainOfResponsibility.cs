
using System.ComponentModel;

namespace Brew.Patterns;

[Description("Chain of Responsibility Pattern")]
public class ChainOfResponsibility : IBrew
{
    public class Request
    {
        public RequestType Type { get; set; }
        public string Details { get; set; }
        public string SignedOffBy { get; set; }
    }

    public enum RequestType
    {
        Level1, // entry level sign off
        Level2, // mid level sign off
        Level3 // management sign off
    }

    public abstract class Employee
    {
        protected Employee? Successor;

        public void SetSuccessor(Employee successor)
        {
            Successor = successor;
        }

        public abstract void SignOff(Request request);
    }

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

    public class Ceo : Employee
    {
        public override void SignOff(Request request)
        {
            if (request.Type == RequestType.Level3)
            {
                // Handle the request
            }
            else Successor?.SignOff(request);
        }
    }
    
    public void After()
    {
        var ceo = new Ceo();
        var midLevel = new MidLevel();
        var entryLevel = new EntryLevel();

        midLevel.SetSuccessor(ceo);
        entryLevel.SetSuccessor(midLevel);

        entryLevel.SignOff(new Request() { Type = RequestType.Level1 });
        entryLevel.SignOff(new Request() { Type = RequestType.Level2 });
        entryLevel.SignOff(new Request() { Type = RequestType.Level3 });
    }

}
