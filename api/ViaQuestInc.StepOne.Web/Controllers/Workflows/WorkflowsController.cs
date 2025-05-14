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
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var workflows = await workflowService.IndexAsync(cancellationToken);

        return Ok(workflows);
    }

    [HttpGet("{workflowId:int}", Name = nameof(ShowWorkflow))]
    public async Task<IActionResult> ShowWorkflow([FromRoute] int workflowId, CancellationToken cancellationToken)
    {
        var workflow = await workflowService.ShowAsync(workflowId, cancellationToken);

        if (workflow == null) return NotFound($"Workflow {workflowId} was not found.");

        return Ok(workflow);
    }

    // TODO - authorization
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Workflow workflow, CancellationToken cancellationToken)
    {
        var createdWorkflow = await workflowService.CreateAsync(workflow, cancellationToken);

        return CreatedAtRoute(nameof(ShowWorkflow), new { workflowId = createdWorkflow.Id }, createdWorkflow);
    }

    // TODO - authorization
    [HttpPatch("{workflowId:int}")]
    public async Task<IActionResult> Update([FromRoute] int workflowId, [FromBody] Workflow updatedWorkflow,
        [FromServices] WorkflowPersistenceEngine workflowPersistenceEngine,
        CancellationToken cancellationToken)
    {
        var originalWorkflow = await workflowService.ShowAsync(workflowId, cancellationToken);

        if (originalWorkflow == null) return NotFound($"Workflow {workflowId} was not found.");

        if (workflowId != updatedWorkflow.Id)
        {
            return BadRequest($"The supplied workflow does not belong to workflow ID {workflowId}.");
        }

        var pipelineOptions = new PipelineOptions(originalWorkflow, updatedWorkflow);

        updatedWorkflow = await workflowPersistenceEngine.ExecuteAsync(pipelineOptions, cancellationToken);

        return Ok(updatedWorkflow);
    }

    [HttpDelete("{workflowId:int}")]
    public async Task<IActionResult> Delete(int workflowId, CancellationToken cancellationToken)
    {
        var workflow = await workflowService.ShowAsync(workflowId, cancellationToken);

        if (workflow == null) return NotFound($"Workflow {workflowId} was not found.");

        await workflowService.DeleteAsync(workflow, cancellationToken);

        return NoContent();
    }
}