using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Workflows.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Workflows;

[Authorize(Policy = Policies.NativeJwtAuthPolicy, Roles = Roles.Internal)]
[Route("job-titles/{jobTitleId:int}/workflow")]
public class JobTitleWorkflowController(JobTitleWorkflowService jobTitleWorkflowService) : ApiControllerBase
{
    [HttpGet(Name = nameof(ShowJobTitleWorkflow))]
    public async Task<IActionResult> ShowJobTitleWorkflow(
        [FromRoute] int jobTitleId,
        CancellationToken cancellationToken)
    {
        var jobTitleWorkflow = await jobTitleWorkflowService.GetByJobTitleIdAsync(jobTitleId, cancellationToken);

        if (jobTitleWorkflow == null) return NotFound($"No workflow assignment found for job title ID {jobTitleId}.");

        return Ok(jobTitleWorkflow);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromRoute] int jobTitleId,
        [FromBody] CreateJobTitleWorkflowRequest jobTitleWorkflowRequest,
        CancellationToken cancellationToken)
    {
        var newJobTitleWorkflow = await jobTitleWorkflowService.CreateAsync(
            jobTitleId,
            jobTitleWorkflowRequest.WorkflowId,
            cancellationToken);

        return CreatedAtRoute(nameof(ShowJobTitleWorkflow), new { jobTitleId }, newJobTitleWorkflow);
    }

    public class CreateJobTitleWorkflowRequest
    {
        public int WorkflowId { get; set; }
    }
}