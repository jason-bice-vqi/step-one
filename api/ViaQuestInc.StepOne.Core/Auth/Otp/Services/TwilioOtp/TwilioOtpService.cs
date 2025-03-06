using Microsoft.Extensions.Options;
using Serilog;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;
using ViaQuestInc.StepOne.Core.Candidates;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;

public class TwilioOtpService(IRepository repository, IOptions<TwilioOtpConfig> twilioConfigOptions) : IOtpService
{
    public async Task<bool> SendOtpAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        var candidate =
            await repository.GetAsync<Candidate>(x => x.MobilePhoneNumber == phoneNumber, cancellationToken);

        //if (candidate is null) return false;

        var fullPhoneNumber = $"+1{phoneNumber}";

        Log.Information("Twilio OTP Requested for {Phone}.", fullPhoneNumber);

        var verification = await VerificationResource.CreateAsync(
            to: fullPhoneNumber,
            channel: "sms",
            pathServiceSid: twilioConfigOptions.Value.VerifyServiceSid
        );

        Log.Information("Twilio OTP Request Result: {Result}", verification.Status);

        return verification.Status == "pending";
    }

    public async Task<Candidate?> ValidateOtpTokenAsync(string phoneNumber, string otp,
        CancellationToken cancellationToken)
    {
        var fullPhoneNumber = $"+1{phoneNumber}";

        Log.Information("Twilio OTP Verification Requested for {Phone}.", fullPhoneNumber);

        try
        {
            var verification = await VerificationCheckResource.CreateAsync(
                to: fullPhoneNumber,
                code: otp,
                pathServiceSid: twilioConfigOptions.Value.VerifyServiceSid
            );

            Log.Information("Twilio OTP Verification Result for {Phone}: {Result}", fullPhoneNumber,
                verification.Status);

            if (verification.Status != "approved") return null;
        }
        catch (ApiException e)
        {
            // This exception is thrown if a bad (invalid or reused) OTP is supplied by the user.
            // Would be better if an exception wasn't generated, and instead Status was used.
            Log.Information("Verification failed for {Phone}.", fullPhoneNumber);

            return null;
        }

        return await repository.GetAsync<Candidate>(x => x.OneTimePasscode == otp && x.MobilePhoneNumber == phoneNumber,
            cancellationToken);
    }
}