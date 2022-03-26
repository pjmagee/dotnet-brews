
using System.Collections.Generic;

namespace Brew.Patterns;

public class SimpleBuilder : IBrew
{
    public void Before()
    {
        Entity entity = new Entity();
        entity.Property1 = true;
        entity.Property2 = "";
        entity.Property3 = new Dictionary<string, string>();
        entity.Numbers.AddRange(new[] { 1, 5 });
    }

    public void After()
    {
        EntityBuilder builder = new();

        Entity entity = builder
            .WithProperty1(true)
            .WithProperty2("property 2")
            .WithProperty3(new KeyValuePair<string, string>("A", "1"), new KeyValuePair<string, string>("B", "2"))
            .WithNumberAdded(1)
            .WithNumberAdded(5)
            .Build();
    }

    public class Entity
    {
        public bool Property1 { get; set; }
        public string Property2 { get; set; }
        public Dictionary<string, string> Property3 { get; set; }
        public List<int> Numbers { get; set; } = new List<int>();
    }

    public class EntityBuilder
    {
        private Entity entity;

        public Entity Build() => entity;

        public EntityBuilder()
        {
            entity = new Entity();
        }

        public EntityBuilder WithProperty1(bool value)
        {
            entity.Property1 = value;
            return this;
        }

        public EntityBuilder WithNumberAdded(int number)
        {
            entity.Numbers.Add(number);
            return this;
        }

        public EntityBuilder WithProperty2(string value)
        {
            entity.Property2 = value;
            return this;
        }

        public EntityBuilder WithProperty3(params KeyValuePair<string, string>[] values)
        {
            entity.Property3 = new Dictionary<string, string>(values);
            return this;
        }
    }
}
