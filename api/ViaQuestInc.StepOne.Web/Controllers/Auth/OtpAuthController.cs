using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;

namespace ViaQuestInc.StepOne.Web.Controllers.Auth;

[AllowAnonymous]
[Route("auth/otp")]
public class OtpAuthController(IOtpService otpService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendOtp([FromBody] OtpRequest otpRequest, CancellationToken cancellationToken)
    {
        var sent = await otpService.SendOtpAsync(otpRequest.PhoneNumber, cancellationToken);

        if (sent) return NoContent();

        return NotFound();
    }

    [HttpPost("exchange")]
    public async Task<IActionResult> Authenticate([FromBody] OtpRequest otpRequest, CancellationToken cancellationToken)
    {
        if (!await otpService.ValidateOtpTokenAsync(otpRequest.PhoneNumber, otpRequest.Otp!, cancellationToken))
            return Unauthorized(new { Message = "Invalid OTP" });

        var jwt = await otpService.GetNativeTokenAsync(otpRequest.PhoneNumber, cancellationToken);

        return Ok(new { Token = jwt });
    }
}