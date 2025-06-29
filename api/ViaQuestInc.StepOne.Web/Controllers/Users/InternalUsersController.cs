using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Users;

[ApiController]
[Authorize(Policies.InternalUserPolicy)]
[Route("users/internal")]
public class InternalUsersController(CandidateWorkflowService candidateWorkflowService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var candidateId = User.GetCandidateId();

        var candidate = await candidateWorkflowService.GetAsync(candidateId!.Value, cancellationToken);

        if (candidate is null) return CandidateNotFound(candidateId.Value);

        return Ok(candidate);
    }
}