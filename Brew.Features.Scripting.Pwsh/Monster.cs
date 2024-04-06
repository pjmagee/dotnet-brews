namespace Brew.Features.Scripting.Pwsh;

public class Monster(string name, int health)
{
    public string Name { get; set; } = name;
    public int Health { get; set; } = health;

    public void DealDamage(Character character, int damage)
    {
        character.Health -= damage;
    }
}