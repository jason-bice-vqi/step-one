using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;

namespace ViaQuestInc.StepOne.Web.Controllers;

[AllowAnonymous]
[Route("otp")]
public class OtpController(IOtpService otpService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendOtp([FromBody] OtpRequest otpRequest, CancellationToken cancellationToken)
    {
        var sent = await otpService.SendOtpAsync(otpRequest.PhoneNumber, cancellationToken);

        if (sent) return NoContent();

        return NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpRequest otpRequest, CancellationToken cancellationToken)
    {
        var candidate =
            await otpService.ValidateOtpTokenAsync(otpRequest.PhoneNumber, otpRequest.Otp!, cancellationToken);

        if (candidate is null) return BadRequest(new { Message = "Invalid OTP" });

        return Ok(candidate);
    }
}