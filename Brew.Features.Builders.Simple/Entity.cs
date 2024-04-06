namespace Brew.Features.Builders.Simple;

public class Entity
{
    public bool Property1 { get; set; }
    public string Property2 { get; set; }
    public Dictionary<string, string> Property3 { get; set; }
    public List<int> Numbers { get; set; } = new List<int>();
}