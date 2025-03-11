﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth.Services;
using ViaQuestInc.StepOne.Web.ServiceModules.Auth;

namespace ViaQuestInc.StepOne.Web.Controllers;

[Route("auth/ad/exchange-token")]
public class AzureAdAuthController(JwtService jwtService) : ControllerBase
{
    [Authorize(Policy = AuthPolicies.InitialAzureAdJwtAuthPolicy)]
    public async Task<IActionResult> ExchangeAdJwtForStepOneJwt(CancellationToken cancellationToken)
    {
        if (User.Identity is not { IsAuthenticated: true })
        {
            return Unauthorized(new
                { Message = "User is not authenticated.", Claims = User.Claims.Select(c => new { c.Type, c.Value }) });
        }

        var jwt = await jwtService.GenerateTokenAsync(User, cancellationToken);

        return Ok(new { Token = jwt });
    }
}