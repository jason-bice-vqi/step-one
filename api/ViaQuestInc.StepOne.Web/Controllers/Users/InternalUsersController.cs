using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Kernel.Auth;
using ViaQuestInc.StepOne.Web.ServiceModules.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.InternalUserPolicy)]
[Route("users/internal")]
public class InternalUsersController(CandidateWorkflowService candidateWorkflowService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var candidateId = User.GetCandidateId();
        
        if (candidateId == null) return Unauthorized("Candidate claim not found."); // TODO - bake this check into the policy
        
        var candidate = await candidateWorkflowService.GetAsync(candidateId!.Value, cancellationToken);

        if (candidate is null) return CandidateNotFound(candidateId!.Value);

        return Ok(candidate);
    }
}