using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Organization.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Org;

[Authorize(Policy = Policies.NativeJwtAuthPolicy, Roles = Roles.Internal)]
[Route("companies/{companyId:int}/job-titles")]
public class JobTitlesController(JobTitlesService jobTitlesService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] int companyId, CancellationToken cancellationToken)
    {
        var companies = await jobTitlesService.IndexAsync(companyId, cancellationToken);

        return Ok(companies);
    }
}