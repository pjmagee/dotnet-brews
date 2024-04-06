namespace Brew.Features.Scripting.Pwsh;

public class Character(string name, int health)
{
    public string Name { get; set; } = name;
    public int Health { get; set; } = health;

    public void DealDamage(Monster monster, int damage)
    {
        monster.Health -= damage;
    }
}