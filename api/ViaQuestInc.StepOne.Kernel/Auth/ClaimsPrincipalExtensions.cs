using System.Security.Claims;

namespace ViaQuestInc.StepOne.Kernel.Auth;

public static class ClaimsPrincipalExtensions
{
    public static bool IsExternal(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(ClaimTypes.Role, Roles.External);
    }
    
    public static bool IsInternal(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(ClaimTypes.Role, Roles.Internal);
    }
    
    public static int? GetCandidateId(this ClaimsPrincipal claimsPrincipal)
    {
        var candidateIdClaim = claimsPrincipal.FindFirstValue(Claims.CandidateId);

        return candidateIdClaim == null ? null : int.Parse(candidateIdClaim);
    }

    public static string? GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}