using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Organization.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Org;

[Authorize(Policy = Policies.NativeJwtAuthPolicy, Roles = Roles.Internal)]
[Route("companies")]
public class CompaniesController(CompanyService companyService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var companies = await companyService.IndexAsync(cancellationToken);

        return Ok(companies);
    }
}