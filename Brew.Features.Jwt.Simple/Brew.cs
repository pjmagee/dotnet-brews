using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Brew.Features.Jwt.Simple;

/// <summary>
/// Demonstrates JWT (JSON Web Token) generation, validation, and claims extraction for secure API authentication.
/// </summary>
public class Brew : ModuleBase
{
    private const string SecretKey = "MySecretKeyForJWTTokensWhichMustBe32BytesLong!";
    private const string Issuer = "MyApp.AuthService";
    private const string Audience = "MyApp.ApiConsumers";

    protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddSingleton<JwtGenerator>().AddSingleton<JwtSecurityTokenHandler>();
    }

    protected override Task ExecuteAsync(CancellationToken token = default)
    {
        Logger.LogInformation("========== JWT TOKEN DEMONSTRATION ==========");
        Logger.LogInformation("Scenario: Generate, validate, and decode JWT tokens for API authentication\n");

        DemonstrateTokenGeneration();
        DemonstrateTokenValidation();
        DemonstrateClaimsExtraction();

        Logger.LogInformation("\n---------- BENEFITS OF JWT ----------");
        Logger.LogInformation("✓ Stateless authentication (no server-side sessions)");
        Logger.LogInformation("✓ Self-contained claims (user info in token)");
        Logger.LogInformation("✓ Cross-platform (works with any language/framework)");
        Logger.LogInformation("✓ Secure with HMAC/RSA signing");
        Logger.LogInformation("✓ Expiration handling built-in");

        return Task.CompletedTask;
    }

    private void DemonstrateTokenGeneration()
    {
        Logger.LogInformation("---------- JWT Token Generation ----------");

        var jwtGenerator = Host.Services.GetRequiredService<JwtGenerator>();
        var token = jwtGenerator.GenerateJwtToken(Issuer, Audience, SecretKey);

        Logger.LogInformation("  Generated Token: {Token}", token);
        Logger.LogInformation("  Token includes: subject, email, product claims, expiration");
    }

    private void DemonstrateTokenValidation()
    {
        Logger.LogInformation("\n---------- JWT Token Validation ----------");

        var jwtGenerator = Host.Services.GetRequiredService<JwtGenerator>();
        var handler = Host.Services.GetRequiredService<JwtSecurityTokenHandler>();

        var token = jwtGenerator.GenerateJwtToken(Issuer, Audience, SecretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            ValidateIssuerSigningKey = true
        };

        try
        {
            var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
            Logger.LogInformation("  ✓ Token validated successfully");
            Logger.LogInformation("  Validated token type: {Type}", validatedToken.GetType().Name);
        }
        catch (Exception ex)
        {
            Logger.LogError("  ✗ Token validation failed: {Message}", ex.Message);
        }
    }

    private void DemonstrateClaimsExtraction()
    {
        Logger.LogInformation("\n---------- JWT Claims Extraction ----------");

        var jwtGenerator = Host.Services.GetRequiredService<JwtGenerator>();
        var handler = Host.Services.GetRequiredService<JwtSecurityTokenHandler>();

        var token = jwtGenerator.GenerateJwtToken(Issuer, Audience, SecretKey);
        var jwtToken = handler.ReadJwtToken(token);

        Logger.LogInformation("  Issuer: {Issuer}", jwtToken.Issuer);
        Logger.LogInformation("  Audience: {Audiences}", string.Join(", ", jwtToken.Audiences));
        Logger.LogInformation("  Valid From: {ValidFrom}", jwtToken.ValidFrom);
        Logger.LogInformation("  Valid To: {ValidTo}", jwtToken.ValidTo);

        Logger.LogInformation("  Claims:");
        foreach (var claim in jwtToken.Claims)
        {
            Logger.LogInformation("    {Type}: {Value}", claim.Type, claim.Value);
        }

        var products = jwtToken.Claims.Where(c => c.Type == "Product").Select(c => c.Value).ToArray();
        Logger.LogInformation("  User has access to products: {Products}", string.Join(", ", products));
    }
}
