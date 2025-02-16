using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Web.ServiceModules;

namespace ViaQuestInc.StepOne.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthModule.OtpScheme, Policy = AuthModule.ExternalUserPolicy)]
[Route("users/external")]
public class ExternalUsersController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}