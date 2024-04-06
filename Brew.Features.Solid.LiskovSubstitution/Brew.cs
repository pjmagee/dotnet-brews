using Brew.Features.Solid.LiskovSubstitution.After;
using Brew.Features.Solid.LiskovSubstitution.Before;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Solid.LiskovSubstitution;

public class Brew : ModuleBase
{
    protected override Task BeforeAsync(CancellationToken token = default)
    {
        Apple apple = new Orange();
        Logger.LogInformation("Apple Color: {Colour}", apple.Colour); // This will be red, but we know its wrong. Poor abstraction and inheritance
        apple.Bite();

        Orange orange = new Orange();
        orange.Squeeze(); // Only possible because not on base class
        
        return Task.CompletedTask;
    }

    protected override Task AfterAsync(CancellationToken token = default)
    {
        Fruit orange = new OrangeFruit();
        Fruit apple = new AppleFruit();

        Logger.LogInformation("Orange color: {Colour}", orange.Colour); // will be Orange
        Logger.LogInformation("Apple colour: {Colour}", apple.Colour); // will be Red
        
        return Task.CompletedTask;
    }

    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}