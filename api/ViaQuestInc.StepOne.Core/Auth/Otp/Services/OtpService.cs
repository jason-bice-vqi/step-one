using ViaQuestInc.StepOne.Core.Candidates;
using ViaQuestInc.StepOne.Kernel;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services;

public class OtpService(IRepository repository) : IOtpService
{
    private const int OtpLength = 6;
    private const char OtpPadChar = '0';
    private const int OtpMax = 999999;
    private const int OtpMin = 0;

    public async Task<string> GenerateOtpAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        var oneTimePasscode = new Random().Next(OtpMin, OtpMax).ToString().PadLeft(OtpLength, OtpPadChar);

        candidate.OneTimePasscode = oneTimePasscode;
        candidate.OneTimePasswordCreatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);

        return oneTimePasscode;
    }

    public Task<bool> EmailOtpAsync(string email, string otp, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SmsOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendOtpAsync(string emailOrPhone, string otp, CancellationToken cancellationToken)
    {
        var inputType = emailOrPhone.GetInputType();

        switch (inputType)
        {
            case InputTypes.Email:
                return await EmailOtpAsync(emailOrPhone, otp, cancellationToken);

            case InputTypes.PhoneNumber:
                return await SmsOtpAsync(emailOrPhone, otp, cancellationToken);

            case InputTypes.Indeterminate:
            default:
                return false;
        }
    }

    public async Task<Candidate?> ValidateOtpTokenAsync(string phoneNumber, string otp,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(otp)) return null;

        var candidate = await repository.GetAsync<Candidate>(
            x => x.OneTimePasscode == otp && x.MobilePhoneNumber == phoneNumber, cancellationToken);

        if (candidate == null) return null;

        candidate.OneTimePasscode = null;
        candidate.OneTimePasswordCreatedAt = null;
        candidate.OneTimePasswordExpiresAt = null;
        candidate.LastAuthenticatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);

        return candidate;
    }
}