using System.ComponentModel;

namespace Brew.Patterns;

[Description("Adaptor Pattern")]
public class Adaptors : IBrew
{
    public void Before()
    {
        var adaptee = new Adaptee();
        var result = "Hello " + adaptee.Hello();
    }

    public void After()
    {
        Adaptor adaptor = new Adaptor(new Adaptee());
        adaptor.Hello();
        adaptor.GetEntity();
    }

    public class Adaptee
    {
        public string Hello() => "World";
        public Entity GetEntity() => new Entity();
    }

    public class Entity
    {
        public string Property { get; set; }
    }

    public class Adaptor
    {
        private readonly Adaptee _adaptee;

        public Adaptor(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }

        // Modify or forward to original function 
        public string Hello() => "Hello " + _adaptee.Hello();

        // Can also Modify or wrap in custom class.
        public Entity GetEntity() => _adaptee.GetEntity();
    }
}
