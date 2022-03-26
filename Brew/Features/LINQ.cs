using System.Collections.Generic;
using System.Linq;

namespace Brew.Features;

public class LINQ : IBrew
{
    public void Before()
    {
        List<Entity> entities = new();

        for (int i = 0; i < 100; i++)
        {
            var entity = new Entity
            {
                Id = i
            };

            entities.Add(entity);
        }

        List<Entity> filtered = new();

        foreach (var entity in entities)
        {
            if (entity.Id >= 50)
                filtered.Add(entity);
        }
    }

    public void After()
    {
        List<Entity> entities = Enumerable
                .Range(0, 100) // The range (for i = 0; i < 100; i++ 
                .Select(i => new Entity() { Id = i }) // Select is like the body in our original for loop 
                .Where(x => x.Id >= 50) // this is like our if statement when adding to our filtered list
                .ToList(); // turn this into concrete List<int>
    }

    public class Entity
    {
        public int Id { get; set; }
    }
}
