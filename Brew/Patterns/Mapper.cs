
using System;

namespace Brew.Patterns;

public class Mapper : IBrew
{
    public class Entity
    {
        public string Name { get; set; }
        public DateTime Age { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    // define custom serialization logic
    public class EntityDto
    {
        public string Name { get; set; }

        public DateTime Age { get; set; }

        public Gender Gender { get; set; }

        public static EntityDto MapFrom(Entity entity)
        {
            return new EntityDto
            {
                Age = entity.Age,
                Gender = entity.Gender,
                Name = entity.Name
            };
        }
    }

    public void Before()
    {
        Entity entity = new Entity();

        // Consume entity at every layer, 
    }


    public void After()
    {
        EntityDto dto = EntityDto.MapFrom(new Entity());

        // Serialize, or use dto to cross boundaries
    }
}
