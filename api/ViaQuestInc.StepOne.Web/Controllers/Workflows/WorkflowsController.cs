using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Core.Workflows.Persistence;
using ViaQuestInc.StepOne.Core.Workflows.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Workflows;

[Authorize(Policy = Policies.NativeJwtAuthPolicy)]
[Route("workflows")]
public class WorkflowsController(WorkflowService workflowService) : ApiControllerBase
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var workflows = await workflowService.GetAsync(cancellationToken);

        return Ok(workflows);
    }

    // TODO - authorization
    [HttpPatch("{workflowId:int}")]
    public async Task<IActionResult> Update([FromRoute] int workflowId, [FromBody] Workflow updatedWorkflow,
        [FromServices] WorkflowPersistenceEngine workflowPersistenceEngine,
        CancellationToken cancellationToken)
    {
        var originalWorkflow = await workflowService.GetAsync(workflowId, cancellationToken);

        if (originalWorkflow == null) return NotFound($"Workflow {workflowId} was not found.");

        if (workflowId != updatedWorkflow.Id)
        {
            return BadRequest($"The supplied workflow does not belong to workflow ID {workflowId}.");   
        }

        var pipelineOptions = new PipelineOptions(originalWorkflow, updatedWorkflow);
        
        updatedWorkflow = await workflowPersistenceEngine.ExecuteAsync(pipelineOptions, cancellationToken);

        return Ok(updatedWorkflow);
    }
}