using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Web.ServiceModules;

namespace ViaQuestInc.StepOne.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthModule.InternalUserPolicy)]
[Route("users/internal")]
public class InternalUsersController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}