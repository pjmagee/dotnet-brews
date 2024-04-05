namespace Brew.Feature.Linq;

public class Brew : IBrew
{
    public void Execute()
    {
        List<Entity> entities = Enumerable
            .Range(0, 100) // The range (for i = 0; i < 100; i++ 
            .Select(i => new Entity() { Id = i }) // Select is like the body in our original for loop 
            .Where(x => x.Id >= 50) // this is like our if statement when adding to our filtered list
            .ToList(); // turn this into concrete List<int>
    }
}