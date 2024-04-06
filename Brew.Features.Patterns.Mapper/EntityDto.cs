namespace Brew.Features.Patterns.Mapper;

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