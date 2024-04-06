using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Brew.Features.Jwt.Simple;

public class JwtGenerator(JwtSecurityTokenHandler handler)
{
    public string GenerateJwtToken(string issuer, string audience, string secretKey)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "testing"),			
            new Claim(JwtRegisteredClaimNames.Email, "testing@testing.com"),
            new Claim("Product", "ProductA"),
            new Claim("Product", "ProductB")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),  // Set token expiration
            signingCredentials: credentials
        );

        return handler.WriteToken(token);
    }
}