using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Patterns.Mapper;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        // Serialize, or use dto to cross boundaries
        EntityDto dto = EntityDto.MapFrom(new Entity());
        
        return Task.CompletedTask;
    }
    
}