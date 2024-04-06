using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brew.Features.Serialization.Avro;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<AvroGenerator>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var avroGenerator = Host.Services.GetRequiredService<AvroGenerator>();
        avroGenerator.Execute();
        
        return Task.CompletedTask;
    }
}