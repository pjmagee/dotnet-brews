using Brew.Features.Solid.InterfaceSegregation.After;
using Brew.Features.Solid.InterfaceSegregation.Before;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.InterfaceSegregation;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        // Before: Fat interface
        services.AddSingleton<IHeroBefore, Diablo>();
        
        // After: Segregated interfaces - each hero only implements what they can do
        services.AddSingleton<IHeroAfter, Genji>();
        services.AddSingleton<IHeroAfter, LtMorales>();
        services.AddSingleton<IHeroAfter, DiabloImproved>();
    }

    protected override Task BeforeAsync(CancellationToken token = default)
    {
        Console.WriteLine("=== BEFORE: Violates Interface Segregation Principle ===\n");
        Console.WriteLine("Problem: IHeroBefore forces ALL heroes to implement ALL methods");
        Console.WriteLine("Even if the hero cannot perform that action!\n");
        
        try
        {
            var heroBefore = Host.Services.GetRequiredService<IHeroBefore>();
            
            Console.WriteLine("Diablo (Tank) forced to implement:");
            heroBefore.Heal();          // ❌ Can't heal - logs error
            heroBefore.Peel();          // ✓ Can peel
            heroBefore.Assassinate();   // ❌ Can't assassinate - logs error
            heroBefore.Gank();          // ❌ Can't gank - logs error
            heroBefore.BasicAttack();   // ✓ Can attack
            
            Console.WriteLine("\n✗ Diablo forced to implement 3 methods he can't perform!");
            Console.WriteLine("✗ Violates ISP: Classes shouldn't depend on methods they don't use\n");
        }
        catch (NotImplementedException e)
        {
            Logger.LogError(e, "Not implemented");
        }
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        Console.WriteLine("\n=== AFTER: Follows Interface Segregation Principle ===\n");
        Console.WriteLine("Solution: Segregated interfaces - heroes only implement what they can do\n");
        
        foreach (var heroAfter in Host.Services.GetServices<IHeroAfter>())
        {
            var heroName = heroAfter.GetType().Name;
            Console.WriteLine($"{heroName} capabilities:");
            
            // All heroes can basic attack
            heroAfter.BasicAttack();
            
            // Only check for capabilities the hero actually has
            if (heroAfter is IHealer healer)
            {
                Console.WriteLine($"  ✓ {heroName} implements IHealer");
                healer.Heal();
            }
            
            if (heroAfter is IAssassin assassin)
            {
                Console.WriteLine($"  ✓ {heroName} implements IAssassin");
                assassin.Assassinate();
            }
            
            if (heroAfter is IGanker ganker)
            {
                Console.WriteLine($"  ✓ {heroName} implements IGanker");
                ganker.Gank();
            }
            
            if (heroAfter is IPeeler peeler)
            {
                Console.WriteLine($"  ✓ {heroName} implements IPeeler");
                peeler.Peel();
            }
            
            Console.WriteLine();
        }
        
        Console.WriteLine("✓ Each hero only implements interfaces for their actual capabilities");
        Console.WriteLine("✓ No forced implementation of irrelevant methods");
        Console.WriteLine("✓ Follows ISP: Clients shouldn't be forced to depend on methods they don't use");
        
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}