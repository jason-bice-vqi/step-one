using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;

namespace ViaQuestInc.StepOne.Web.Controllers;

[AllowAnonymous]
[Route("auth/otp")]
public class OtpAuthController(IOtpService otpService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendOtp([FromBody] OtpRequest otpRequest, CancellationToken cancellationToken)
    {
        var sent = await otpService.SendOtpAsync(otpRequest.PhoneNumber, cancellationToken);

        if (sent) return NoContent();

        return NotFound();
    }

    [HttpPost("challenge")]
    public async Task<IActionResult> Authenticate([FromBody] OtpRequest otpRequest, CancellationToken cancellationToken)
    {
        var jwt = await otpService.ValidateOtpTokenAsync(otpRequest.PhoneNumber, otpRequest.Otp!, cancellationToken);

        if (jwt == null) return Unauthorized(new { Message = "Invalid OTP" });

        return Ok(new { Token = jwt });
    }
}