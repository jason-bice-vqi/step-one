using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Organization.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Org;

[Authorize(Policy = Policies.NativeJwtAuthPolicy, Roles = Roles.Internal)]
[Route("job-titles")]
public class JobTitleAliasesController(JobTitleAliasService jobTitlesAliasService) : ApiControllerBase
{
    [HttpGet("{jobTitleId:int}/aliases")]
    public async Task<IEnumerable<JobTitleAlias>> Index(int jobTitleId, CancellationToken cancellationToken)
    {
        return await jobTitlesAliasService.IndexAsync(jobTitleId, cancellationToken);
    }

    [HttpPost("{jobTitleId:int}/aliases")]
    public async Task<IActionResult> Create(
        [FromRoute] int jobTitleId,
        [FromBody] CreateJobTitleAliasRequest jobTitleAliasRequest,
        CancellationToken cancellationToken)
    {
        var jobTitleAlias = await jobTitlesAliasService.CreateAsync(
            jobTitleId,
            jobTitleAliasRequest.Alias,
            cancellationToken);

        return CreatedAtRoute(nameof(ShowJobTitleAlias), new { jobTitleAliasId = jobTitleAlias.Id }, jobTitleAlias);
    }

    [HttpGet("aliases/{jobTitleAliasId:int}", Name = nameof(ShowJobTitleAlias))]
    public async Task<IActionResult> ShowJobTitleAlias(
        [FromRoute] int jobTitleAliasId,
        CancellationToken cancellationToken)
    {
        var jobTitleAlias = await jobTitlesAliasService.ShowAsync(jobTitleAliasId, cancellationToken);

        if (jobTitleAlias == null) return NotFound();

        return Ok(jobTitleAlias);
    }

    [HttpDelete("aliases/{jobTitleAliasId:int}")]
    public async Task<IActionResult> Delete([FromRoute] int jobTitleAliasId, CancellationToken cancellationToken)
    {
        var jobTitleAlias = await jobTitlesAliasService.ShowAsync(jobTitleAliasId, cancellationToken);

        if (jobTitleAlias == null) return NotFound();

        await jobTitlesAliasService.DeleteAsync(jobTitleAlias, cancellationToken);

        return NoContent();
    }

    public class CreateJobTitleAliasRequest
    {
        public required string Alias { get; set; }
    }
}