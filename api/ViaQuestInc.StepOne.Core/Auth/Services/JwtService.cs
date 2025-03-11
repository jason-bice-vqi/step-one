using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ViaQuestInc.StepOne.Core.Candidates;

namespace ViaQuestInc.StepOne.Core.Auth.Services;

public class JwtService(AuthConfig authConfig)
{
    private readonly string secret = authConfig.Jwt.Key;
    private readonly string issuer = authConfig.Jwt.Issuer;

    private static readonly string[] CopyAzureAdClaimNames = [
        JwtRegisteredClaimNames.GivenName,
        JwtRegisteredClaimNames.Name,
        JwtRegisteredClaimNames.Sub,
        JwtRegisteredClaimNames.FamilyName
    ];

    public Task<string> GenerateTokenAsync(ClaimsPrincipal fromAdClaimsPrincipal, CancellationToken cancellationToken)
    {
        var claims = fromAdClaimsPrincipal.Claims.Where(x => CopyAzureAdClaimNames.Contains(x.Type)).ToList(); 
        
        // TODO - flesh out roles/claims
        claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

        return GenerateTokenAsync(claims, cancellationToken);
    }

    public Task<string> GenerateTokenAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.GivenName, candidate.FirstName),
            new(JwtRegisteredClaimNames.Name, candidate.FullName),
            new(JwtRegisteredClaimNames.Sub, candidate.Id.ToString()),
            new(JwtRegisteredClaimNames.FamilyName, candidate.LastName),
            new(ClaimTypes.Role, "New Employee")
        };

        return GenerateTokenAsync(claims, cancellationToken);
    }
    
    private Task<string> GenerateTokenAsync(IEnumerable<Claim> withClaims, CancellationToken cancellationToken)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer,
            issuer,
            withClaims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: signingCredentials
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenStr);
    }
}