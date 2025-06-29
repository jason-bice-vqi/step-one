using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Workflows;

[Authorize(Policy = Policies.NativeJwtAuthPolicy)]
[Route("candidates/{candidateId:int}/workflow")]
public class CandidateWorkflowsController(
    CandidateWorkflowService candidateWorkflowService,
    IAuthorizationService authorizationService
)
    : ApiControllerBase
{
    public async Task<IActionResult> Get([FromRoute] int candidateId, CancellationToken cancellationToken)
    {
        var candidateWithWorkflow = await candidateWorkflowService.GetWithWorkflowAsync(candidateId, cancellationToken);

        if (candidateWithWorkflow is null) return CandidateNotFound(candidateId);

        if (candidateWithWorkflow.CandidateWorkflow is null) return CandidateWorkflowNotFound(candidateId);

        var authorization = await authorizationService.AuthorizeAsync(
            User,
            candidateWithWorkflow.CandidateWorkflow,
            Policies.CandidateWorkflows.CanAccessCandidateWorkflow);

        return authorization.Succeeded ? Ok(candidateWithWorkflow) : Forbid(authorization);
    }
}