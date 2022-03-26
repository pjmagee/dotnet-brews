
using System;

namespace Brew.Patterns;

public class Factory : IBrew
{
    public void Before()
    {
        BeforeConsumer consumer = new BeforeConsumer();
        consumer.DoSomething();
    }

    public void After()
    {
        AfterConsumer consumer = new AfterConsumer(new EntityFactory());
        consumer.DoSomething();
    }

    public class BeforeConsumer
    {
        private Entity _entity;

        public BeforeConsumer()
        {
            // New entity within the consumer
            _entity = new Entity();
        }

        public void DoSomething() => Console.WriteLine(_entity.ToString());
    }

    public class AfterConsumer
    {
        private Entity _entity;

        public AfterConsumer(EntityFactory factory)
        {
            _entity = factory.Create(1);
        }

        public void DoSomething() => Console.WriteLine(_entity.ToString());
    }

    public class EntityFactory
    {
        public Entity Create(int type)
        {
            return type switch
            {
                1 => new Entity(), // type 1 based on argument
                2 => new Entity() // type 2 based on argument
            };
        }
    }

    public class Entity
    {

    }
}
