using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Candidates.Services;

namespace ViaQuestInc.StepOne.Web.Controllers;

[Route("candidates/{candidateId:guid}/workflow")]
public class CandidateWorkflowsController(CandidateService candidateService) : ControllerBase
{
    // TODO - ensure the user requesting this should have access to it: either an admin or the candidate who owns the workflow
    public async Task<IActionResult> Get([FromRoute] Guid candidateId, CancellationToken cancellationToken)
    {
        var candidateWithWorkflow = await candidateService.GetWithWorkflowAsync(candidateId, cancellationToken);
        
        if (candidateWithWorkflow is null) return NotFound($"Candidate '{candidateId}' not found.");

        if (candidateWithWorkflow.CandidateWorkflow is null)
        {
            return NotFound($"Workflow not found for candidate '{candidateId}'.");
        }

        return Ok(candidateWithWorkflow);
    }
}