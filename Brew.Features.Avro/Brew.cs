using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Avro;

public class Brew : IBrew
{
    public void Execute()
    {
        using(var host = Host.CreateDefaultBuilder().ConfigureLogging(x => x.AddConsole()).ConfigureServices(collection => collection.AddSingleton<AvroGenerator>()).Build())
        {
            using(var scope = host.Services.CreateScope())
            {
                var avroGenerator = scope.ServiceProvider.GetRequiredService<AvroGenerator>();
                avroGenerator.Execute();
            }
        }
    }
}