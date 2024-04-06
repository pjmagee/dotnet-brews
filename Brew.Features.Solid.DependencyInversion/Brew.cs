using Brew.Features.Solid.DependencyInversion.After;
using Brew.Features.Solid.DependencyInversion.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Solid.DependencyInversion;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
       
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        OrderServiceFlexible serviceFlexible = new OrderServiceFlexible(new OrderRepository(new List<Order>()));
        serviceFlexible.AddOrder(new Order());

        return Task.CompletedTask;
    }
}