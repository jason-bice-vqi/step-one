using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers.Auth;

[Authorize(Policies.AzureAdJwtAuthPolicy)]
[Route("auth/ad/exchange")]
public class AzureAdAuthController(JwtService jwtService) : ApiControllerBase
{
    public IActionResult ExchangeAdJwtForStepOneJwt()
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Unauthorized(
                new
                {
                    Message = "User is not authenticated.", Claims = User.Claims.Select(c => new { c.Type, c.Value })
                });

        var jwt = jwtService.GenerateToken(User);

        return Ok(new { Token = jwt });
    }
}