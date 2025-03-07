// using System.Security.Claims;
// using System.Text.Encodings.Web;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.Extensions.Options;
// using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
//
// namespace ViaQuestInc.StepOne.Web.Auth;
//
// public class OtpAuthenticationHandler(
//     IOptionsMonitor<AuthenticationSchemeOptions> options,
//     ILoggerFactory logger,
//     UrlEncoder encoder,
//     IOtpService otpService)
//     : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
// {
//     protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//     {
//         var oneTimePasscode = Request.Headers.Authorization.SingleOrDefault()?.Replace("Bearer ", "");
//         var mobilePhoneNumber = Request.Form["mobilePhoneNumber"];
//
//         if (string.IsNullOrEmpty(oneTimePasscode)) return AuthenticateResult.Fail("No OTP token provided.");
//
//         var user = await otpService.ValidateOtpTokenAsync(mobilePhoneNumber, oneTimePasscode, CancellationToken.None);
//
//         if (user == null) return AuthenticateResult.Fail("Invalid OTP token.");
//
//         var claims = new Claim[]
//         {
//             new(ClaimTypes.Name, user.FullName),
//             new(ClaimTypes.Role, "ExternalUser")
//         };
//
//         var identity = new ClaimsIdentity(claims, "OtpScheme");
//         var principal = new ClaimsPrincipal(identity);
//         var ticket = new AuthenticationTicket(principal, "OtpScheme");
//
//         return AuthenticateResult.Success(ticket);
//     }
// }