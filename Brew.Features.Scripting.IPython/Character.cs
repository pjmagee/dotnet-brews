namespace Brew.Features.Scripting.IPython;

public class Character
{
    public string Name { get; set; }
    public int Health { get; set; }
    public Character(string name, int health)
    {
        Name = name;
        Health = health;
    }

    public void DealDamage(Monster monster, int damage)
    {
        monster.Health -= damage;
        Console.WriteLine($"{Name} deals {damage} damage to {monster.Name}");
    }
}