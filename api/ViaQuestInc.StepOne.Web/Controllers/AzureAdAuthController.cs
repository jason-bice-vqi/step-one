using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
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

    private static readonly string[] ScopeRequiredByApi = new string[] {  };


    [HttpPost("v")]
    public IActionResult AuthV()
    {
        if (User.Identity is not { IsAuthenticated: true })
        {
            return Unauthorized(new
                { Message = "User is not authenticated.", Claims = User.Claims.Select(c => new { c.Type, c.Value }) });
        }

        var cp = User;
        HttpContext.VerifyUserHasAnyAcceptedScope(ScopeRequiredByApi);

        return Ok(new { Message = "User authenticated successfully", User = User.Identity?.Name });
    }
}