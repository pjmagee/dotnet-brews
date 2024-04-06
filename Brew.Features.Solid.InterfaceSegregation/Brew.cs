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
       services
           .AddSingleton<IHeroBefore, Diablo>()
           .AddSingleton<IHeroAfter, Genji>()
           .AddSingleton<IHeroAfter, LtMorales>();
   }

   protected override Task BeforeAsync(CancellationToken token = default)
   {
      try
      {
         var heroBefore = Host.Services.GetRequiredService<IHeroBefore>();
            
         heroBefore.Heal();
         heroBefore.Peel();
         heroBefore.Assassinate();
         heroBefore.Gank();
         heroBefore.BasicAttack();
      }
      catch (NotImplementedException e)
      {
         Logger.LogError(e, "Not implemented");
      }
      
      return Task.CompletedTask;
   }

   protected override Task AfterAsync(CancellationToken token = default)
   {
      foreach (var heroAfter in Host.Services.GetServices<IHeroAfter>())
      {
         heroAfter.BasicAttack();
               
         if (heroAfter is IHealer healer) healer.Heal();
         if (heroAfter is IAssassin assassin) assassin.Assassinate();
         if (heroAfter is IGanker ganker) ganker.Gank();
      }
      
      return Task.CompletedTask;
   }

   protected override Task ExecuteAsync(CancellationToken token = default)
   {
      return Task.CompletedTask;
   }
}