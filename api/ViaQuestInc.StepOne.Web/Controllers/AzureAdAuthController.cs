using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth.AzureActiveDirectory.Services;

namespace ViaQuestInc.StepOne.Web.Controllers;

[AllowAnonymous]
[Route("auth/ad")]
public class AzureAdAuthController(AzureAdAuthService azureAdAuthService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Authenticate(CancellationToken cancellationToken)
    {
        var jwt = await azureAdAuthService.AuthenticateUserAsync(cancellationToken);

        if (jwt == null) return Unauthorized(new { Message = "Invalid credentials" });

        return Ok(new { Token = jwt });
    }
}