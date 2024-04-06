using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew;

public abstract class ModuleBase : IBrew
{
    protected IHost Host { get; }
    public static ILogger Logger { get; private set; } = null!;

    public string? Description { get; } = (Assembly
        .GetExecutingAssembly()
        .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
        .FirstOrDefault() as AssemblyDescriptionAttribute)?.Description;

    protected IHostBuilder CreateHostBuilder()
    {
        return Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(x => x.AddLogging())
            .ConfigureServices(ConfigureServices);
    }

    protected ModuleBase()
    {
        Host = CreateHostBuilder().Build();
        Logger = Host.Services.GetRequiredService<ILogger<ModuleBase>>();
    }

    protected abstract void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services);

    protected virtual Task BeforeAsync(CancellationToken token = default) => Task.CompletedTask;
    protected virtual Task AfterAsync(CancellationToken token = default) => Task.CompletedTask;
    protected abstract Task ExecuteAsync(CancellationToken token = default);

    public virtual async Task RunAsync(CancellationToken token = default)
    {
        await BeforeAsync(token);
        await ExecuteAsync(token);
        await AfterAsync(token);
    }
}