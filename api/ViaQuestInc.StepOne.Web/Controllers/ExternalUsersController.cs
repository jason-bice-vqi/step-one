using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Web.ServiceModules;
using ViaQuestInc.StepOne.Web.ServiceModules.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthModule.OtpScheme, Policy = AuthPolicies.ExternalUserPolicy)]
[Route("users/external")]
public class ExternalUsersController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}