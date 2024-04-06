namespace Brew.Features.Scripting.IPython;

public class Monster
{
    public string Name { get; set; }
    public int Health { get; set; }
    public Monster(string name, int health)
    {
        Name = name;
        Health = health;
    }

    public void DealDamage(Character character, int damage)
    {
        character.Health -= damage;
        Console.WriteLine($"{Name} deals {damage} damage to {character.Name}");
    }
}