using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Workflows.Steps;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Workflows;

[Authorize(Policy = Policies.NativeJwtAuthPolicy)]
[Route("steps")]
public class StepsController(StepService stepService) : ApiControllerBase
{
    // TODO - authorize permission to do this
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Step step, CancellationToken cancellationToken)
    {
        var id = await stepService.CreateAsync(step, cancellationToken);

        return CreatedAtRoute(nameof(Show), new { stepId = id }, step);
    }
    
    [HttpDelete("{stepId:int}")]
    public async Task<IActionResult> Delete(int stepId, CancellationToken cancellationToken)
    {
        var step = await stepService.ShowAsync(stepId, cancellationToken);

        if (step == null) return NotFound($"Step {stepId} was not found.");

        await stepService.DeleteAsync(step, cancellationToken);

        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var steps = await stepService.IndexAsync(cancellationToken);

        return Ok(steps);
    }
    
    [HttpGet("{stepId:int}", Name = nameof(Show))]
    public async Task<IActionResult> Show(int stepId, CancellationToken cancellationToken)
    {
        var step = await stepService.ShowAsync(stepId, cancellationToken);

        if (step == null) return NotFound($"Step {stepId} was not found.");

        return Ok(step);
    }
}