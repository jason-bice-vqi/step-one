using System.Security.Claims;

namespace ViaQuestInc.StepOne.Kernel.Auth;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(ClaimTypes.Role, Claims.Roles.Administrator);
    }
    
    public static int? GetCandidateId(this ClaimsPrincipal claimsPrincipal)
    {
        var candidateIdClaim = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == Claims.CandidateId);

        return candidateIdClaim == null ? null : int.Parse(candidateIdClaim.Value);
    }
}