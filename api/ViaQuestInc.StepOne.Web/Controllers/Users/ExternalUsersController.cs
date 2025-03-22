using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Users;

[ApiController]
[Authorize(Policies.ExternalUserPolicy)]
[Route("users/external")]
public class ExternalUsersController : ApiControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}