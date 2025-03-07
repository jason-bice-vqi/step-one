using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ViaQuestInc.StepOne.Core.Auth.Services;

public class JwtService(AuthConfig authConfig)
{
    private readonly string secret = authConfig.Jwt.Key;
    private readonly string issuer = authConfig.Jwt.Issuer;
    
    public Task<string> GenerateTokenAsync(string userId, string phoneNumber)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.GivenName, "Jason"),
            new Claim(ClaimTypes.Surname, "Bice"),
            new Claim(ClaimTypes.Name, $"Jason Bice"),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, phoneNumber),
            new Claim(ClaimTypes.Role, "User"), // Add roles or other claims as needed
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: signingCredentials
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenStr);
    }
}