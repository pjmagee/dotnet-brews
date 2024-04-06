using System.CodeDom;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Types;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<CSharpCodeProvider>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var provider = Host.Services.GetRequiredService<CSharpCodeProvider>();
        var mscorlib = Assembly.GetAssembly(typeof(int))!;
            
        foreach (var item in mscorlib.DefinedTypes
                     .Where(t => t.Namespace == nameof(System))
                     .Select(type => new { Type = type, Out = provider.GetTypeOutput(new CodeTypeReference(type)) })
                     .Where(x => x.Out.IndexOf('.') == -1)
                     .ToDictionary(x => x.Out, x => x.Type))
        {
            Logger.LogInformation("Alias: {Alias} Type: {Type}", item.Key, item.Value);
        }
        
        return Task.CompletedTask;
    }
}