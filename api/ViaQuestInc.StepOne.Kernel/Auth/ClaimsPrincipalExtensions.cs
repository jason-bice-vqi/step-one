using System.Security.Claims;

namespace ViaQuestInc.StepOne.Kernel.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetCandidateId(this ClaimsPrincipal claimsPrincipal)
    {
        var candidateIdClaim = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == Claims.CandidateId);

        return candidateIdClaim == null ? null : Guid.Parse(candidateIdClaim.Value);
    }
}