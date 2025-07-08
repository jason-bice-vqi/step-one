using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Organization.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Org;

[Authorize(Policy = Policies.NativeJwtAuthPolicy, Roles = Roles.Internal)]
public class JobTitlesController(JobTitleService jobTitleService) : ApiControllerBase
{
    [HttpGet("job-titles")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var companies = await jobTitleService.IndexAsync(cancellationToken);

        return Ok(companies);
    }

    [HttpGet("companies/{companyId:int}/job-titles")]
    public async Task<IActionResult> Index([FromRoute] int companyId, CancellationToken cancellationToken)
    {
        var companies = await jobTitleService.IndexAsync(companyId, cancellationToken);

        return Ok(companies);
    }

    [HttpPatch("job-titles/{jobTitleId:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int jobTitleId,
        [FromBody] UpdateJobTitleRequest updateJobTitleRequest,
        CancellationToken cancellationToken)
    {
        var jobTitle = await jobTitleService.GetAsync(jobTitleId, cancellationToken);

        if (jobTitle == null) return NotFound($"Job title {jobTitleId} not found.");

        await jobTitleService.UpdateAsync(jobTitle, updateJobTitleRequest, cancellationToken);

        return Ok(jobTitle);
    }
}