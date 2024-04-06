using Microsoft.Extensions.Logging;

namespace Brew.Features.Mef;

using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

public class CalculatorOperationsLoader(ILogger<CalculatorOperationsLoader> logger, string pluginsPath)
{
    public CompositionHost LoadPlugins()
    {
        var assemblies = new List<Assembly>();
        var pluginFiles = Directory.GetFiles(pluginsPath, "Brew.Features.Mef.Operations.*.dll", SearchOption.TopDirectoryOnly);

        foreach (var pluginPath in pluginFiles)
        {
            try
            {
                var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(pluginPath);
                assemblies.Add(asm);
                
                logger.LogInformation("Loaded plugin assembly {PluginPath}", pluginPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading plugin assembly {PluginPath}", pluginPath);
                
            }
        }

        var configuration = new ContainerConfiguration().WithAssemblies(assemblies);
        var container = configuration.CreateContainer();
        return container;
    }
}


public class Calculator(CalculatorOperationsLoader operationsLoader, ILogger<Calculator> logger)
{
    public IEnumerable<ICalculatorOperation> Operatons => operationsLoader.LoadPlugins().GetExports<ICalculatorOperation>();
    
    public double Calculate(ICalculatorOperation operation, double arg1, double arg2)
    {
        var result = operation.Operate(arg1, arg2);
        logger.LogInformation("Result of {Operation} operation on {Arg1} and {Arg2} is {Result}", operation, arg1, arg2, result);
        return result;
    }
}