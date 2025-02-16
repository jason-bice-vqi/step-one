using ViaQuestInc.StepOne.Core.Candidates;
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

    public async Task<Candidate?> ValidateOtpTokenAsync(string mobilePhoneNumber, string otpChallenge,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(mobilePhoneNumber) || string.IsNullOrWhiteSpace(otpChallenge)) return null;
        
        var candidate = await repository.GetAsync<Candidate>(
            x => x.OneTimePasscode == otpChallenge && x.MobilePhoneNumber == mobilePhoneNumber, cancellationToken);

        if (candidate == null) return null;

        candidate.OneTimePasscode = null;
        candidate.OneTimePasswordCreatedAt = null;
        candidate.OneTimePasswordExpiresAt = null;
        candidate.LastAuthenticatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
        
        return candidate;
    }
}