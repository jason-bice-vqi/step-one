using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ViaQuestInc.StepOne.Core.Candidates;
using ViaQuestInc.StepOne.Kernel.Auth;

namespace ViaQuestInc.StepOne.Core.Auth.Services;

public class JwtService(AuthConfig authConfig)
{
    private static readonly string[] CopyAzureAdClaimNames =
    [
        JwtRegisteredClaimNames.GivenName,
        JwtRegisteredClaimNames.Name,
        JwtRegisteredClaimNames.Sub,
        JwtRegisteredClaimNames.FamilyName
    ];

    public Task<string> GenerateTokenAsync(ClaimsPrincipal fromAdClaimsPrincipal, CancellationToken cancellationToken)
    {
        var claims = fromAdClaimsPrincipal.Claims.Where(x => CopyAzureAdClaimNames.Contains(x.Type)).ToList();

        // TODO - flesh out roles/claims
        claims.Add(new(ClaimTypes.Role, Claims.Roles.Administrator));

        return GenerateTokenAsync(claims, authConfig.Jwt.LifetimeHoursInternal, cancellationToken);
    }

    public Task<string> GenerateTokenAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        var claims = new Claim[]
        {
            new(Claims.CandidateId, candidate.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, candidate.FirstName),
            new(JwtRegisteredClaimNames.Name, candidate.FullName),
            new(JwtRegisteredClaimNames.Sub, candidate.Id.ToString()),
            new(JwtRegisteredClaimNames.FamilyName, candidate.LastName),
            new(ClaimTypes.Role, "New Employee")
        };

        return GenerateTokenAsync(claims, authConfig.Jwt.LifetimeHoursInternal, cancellationToken);
    }

    private Task<string> GenerateTokenAsync(IEnumerable<Claim> withClaims, int lifetimeHours,
        CancellationToken cancellationToken)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Jwt.Key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            authConfig.Jwt.Issuer,
            authConfig.Jwt.Audience,
            withClaims,
            expires: DateTime.UtcNow.AddHours(lifetimeHours),
            signingCredentials: signingCredentials
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenStr);
    }
}