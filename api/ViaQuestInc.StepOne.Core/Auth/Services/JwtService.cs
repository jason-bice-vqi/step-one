using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ViaQuestInc.StepOne.Core.Candidates;

namespace ViaQuestInc.StepOne.Core.Auth.Services;

public class JwtService(AuthConfig authConfig)
{
    private static readonly string[] CopyAzureAdClaimNames =
    [
        ClaimTypes.NameIdentifier,
        JwtRegisteredClaimNames.FamilyName,
        JwtRegisteredClaimNames.GivenName,
        JwtRegisteredClaimNames.Name,
        JwtRegisteredClaimNames.Sub
    ];

    public string GenerateToken(ClaimsPrincipal fromAdClaimsPrincipal)
    {
        var claims = fromAdClaimsPrincipal.Claims.Where(x => CopyAzureAdClaimNames.Contains(x.Type))
            .ToList();

        claims.Add(new(ClaimTypes.Role, Roles.Internal));

        return GenerateToken(claims, authConfig.Jwt.LifetimeHoursInternal);
    }

    public string GenerateToken(Candidate candidate)
    {
        var claims = new List<Claim>
        {
            new(Claims.CandidateId, candidate.Id.ToString()),
            new(ClaimTypes.NameIdentifier, candidate.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, candidate.FirstName),
            new(JwtRegisteredClaimNames.Name, candidate.FullName),
            new(JwtRegisteredClaimNames.Sub, candidate.Id.ToString()),
            new(JwtRegisteredClaimNames.FamilyName, candidate.LastName),
            new(ClaimTypes.Role, Roles.External)
        };

        if (candidate.CandidateWorkflowId != null)
            claims.Add(new(Claims.CandidateWorkflowId, candidate.CandidateWorkflowId.ToString()!));

        return GenerateToken(claims, authConfig.Jwt.LifetimeHoursInternal);
    }

    private string GenerateToken(IEnumerable<Claim> withClaims, int lifetimeHours)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Jwt.Key));

        var token = new JwtSecurityToken(
            authConfig.Jwt.Issuer,
            authConfig.Jwt.Audience,
            withClaims,
            expires: DateTime.UtcNow.AddHours(lifetimeHours),
            signingCredentials: new(key, SecurityAlgorithms.HmacSha256)
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenStr;
    }
}