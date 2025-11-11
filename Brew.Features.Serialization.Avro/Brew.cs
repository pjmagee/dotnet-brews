using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Serialization.Avro;

/// <summary>
/// Demonstrates Apache Avro serialization: code generation from schema, compact binary serialization for big data
/// </summary>
public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<AvroGenerator>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== APACHE AVRO SERIALIZATION DEMONSTRATION ==========");
        Logger.LogInformation("Scenario: Generate C# models from Avro schemas for efficient data serialization\n");

        var avroGenerator = Host.Services.GetRequiredService<AvroGenerator>();

        Logger.LogInformation("---------- Avro Schema to Code Generation ----------");
        Logger.LogInformation("  Schemas defined: Enum (Suit), Record (Person)");
        avroGenerator.Execute();
        Logger.LogInformation("  ✓ C# models generated from Avro schemas");

        Logger.LogInformation("\n---------- BENEFITS OF AVRO ----------");
        Logger.LogInformation("✓ Compact binary format (smaller than JSON/XML)");
        Logger.LogInformation("✓ Schema evolution (add/remove fields safely)");
        Logger.LogInformation("✓ Cross-language compatibility (Java, C#, Python, etc.)");
        Logger.LogInformation("✓ Code generation from schema definitions");
        Logger.LogInformation("✓ Ideal for big data pipelines (Kafka, Hadoop)");
        Logger.LogInformation("✓ Built-in compression support");

        return Task.CompletedTask;
    }
}
