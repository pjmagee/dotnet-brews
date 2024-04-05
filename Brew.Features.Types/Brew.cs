using System.CodeDom;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Types;

public class Brew : IBrew
{
    public Task Execute()
    {
        using (var host = Host
                   .CreateDefaultBuilder()
                   .ConfigureServices(x => x.AddSingleton<CSharpCodeProvider>())
                   .Build())
        {
            var provider = host.Services.GetRequiredService<CSharpCodeProvider>();
            var mscorlib = Assembly.GetAssembly(typeof(int));
            var logger = host.Services.GetRequiredService<ILogger<Brew>>();
            
            foreach (var item in mscorlib.DefinedTypes
                         .Where(t => t.Namespace == "System")
                         .Select(type => new { Type = type, Out = provider.GetTypeOutput(new CodeTypeReference(type)) })
                         .Where(x => x.Out.IndexOf('.') == -1)
                         .ToDictionary(x => x.Out, x => x.Type))
            {
                logger.LogInformation("Alias: {Alias} Type: {Type}", item.Key, item.Value);
            }
        }

        return Task.CompletedTask;
    }
}