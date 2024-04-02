using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.Extensions.Logging;

namespace Brew.Features;

public class TypeInfos(ILogger<TypeInfos> logger) : IBrew
{
    public void Execute()
    {
        using (var provider = new CSharpCodeProvider())
        {
            var mscorlib = Assembly.GetAssembly(typeof(int));
            var aliases = new Dictionary<string, string>();

            foreach (var item in mscorlib
                         .DefinedTypes
                         .Where(t => t.Namespace == "System")
                         .Select(type => new { Type = type, Out = provider.GetTypeOutput(new CodeTypeReference(type)) })
                         .Where(x => x.Out.IndexOf('.') == -1)
                         .ToDictionary(x => x.Out, x => x.Type))
            {
                logger.LogInformation("Alias: {Alias} Type: {Type}", item.Key, item.Value);
            }
                
                

        }
    }
}