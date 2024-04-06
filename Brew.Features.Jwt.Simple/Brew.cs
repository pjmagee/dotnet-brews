using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Jwt.Simple;

public class Brew : ModuleBase
{
    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<JwtGenerator>().AddSingleton<JwtSecurityTokenHandler>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        var jwtGenerator = Host.Services.GetRequiredService<JwtGenerator>();
        var jwt = jwtGenerator.GenerateJwtToken("testing", "testing", "123456789123456789123456789123456789");
        Logger.LogInformation("Token: {Token}", token);
        return Task.CompletedTask;       
    }
}