using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Kernel.Auth;
using ViaQuestInc.StepOne.Web.ServiceModules.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.InternalUserPolicy)]
[Route("users/internal")]
public class InternalUsersController(CandidateService candidateService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var candidateId = User.GetCandidateId();
        
        if (candidateId == null) return Unauthorized("Candidate claim not found."); // TODO - bake this check into the policy
        
        var candidate = await candidateService.GetAsync(candidateId!.Value, cancellationToken);

        if (candidate is null) return NotFound($"Candidate '{candidateId}' not found."); // TODO - standardize this in a parent controller

        return Ok(candidate);
    }
}