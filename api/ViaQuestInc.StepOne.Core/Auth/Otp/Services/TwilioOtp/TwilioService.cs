using Serilog;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;
using ViaQuestInc.StepOne.Core.Auth.Services;
using ViaQuestInc.StepOne.Core.Candidates;
using ViaQuestInc.StepOne.Core.Candidates.Services;

namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;

public class TwilioService(
    CandidateService candidateService,
    JwtService jwtService,
    AuthConfig authConfig) : IOtpService
{
    public async Task<bool> SendOtpAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        var candidate = await candidateService.GetByPhoneNumberAsync(phoneNumber, cancellationToken);

        if (candidate is null) return false;

        var fullPhoneNumber = $"+1{phoneNumber}";

        Log.Information("Twilio OTP Requested for {Phone}.", fullPhoneNumber);

        var verification = await VerificationResource.CreateAsync(
            to: fullPhoneNumber,
            channel: "sms",
            pathServiceSid: authConfig.Twilio.VerifyServiceSid
        );

        Log.Information("Twilio OTP Request Result: {Result}", verification.Status);

        if (verification.Status == "pending")
        {
            await candidateService.RecordOtpRequestedAsync(candidate, cancellationToken);

            return true;
        }

        return false;
    }

    public async Task<bool> ValidateOtpTokenAsync(string phoneNumber, string otp,
        CancellationToken cancellationToken)
    {
        var fullPhoneNumber = $"+1{phoneNumber}";

        Log.Information("Twilio OTP Verification Requested for {Phone}.", fullPhoneNumber);

        try
        {
            var verification = await VerificationCheckResource.CreateAsync(
                to: fullPhoneNumber,
                code: otp,
                pathServiceSid: authConfig.Twilio.VerifyServiceSid
            );

            Log.Information("Twilio OTP Verification Result for {Phone}: {Result}", fullPhoneNumber,
                verification.Status);

            return verification.Status == "approved";
        }
        catch (ApiException)
        {
            // This exception is thrown if a bad (invalid or reused) OTP is supplied by the user.
            // Would be better if an exception wasn't generated, and instead Status was used.
            Log.Information("Verification failed for {Phone}.", fullPhoneNumber);

            return false;
        }
    }

    public async Task<string> GetNativeTokenAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        var candidate = (await candidateService.GetByPhoneNumberAsync(phoneNumber, cancellationToken))!;

        await candidateService.RecordAuthenticatedAsync(candidate, cancellationToken);
            
        var tokenStr = await jwtService.GenerateTokenAsync(candidate, cancellationToken);

        return tokenStr;
    }
}