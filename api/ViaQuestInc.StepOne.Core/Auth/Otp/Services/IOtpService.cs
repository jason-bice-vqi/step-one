using ViaQuestInc.StepOne.Core.Candidates;

namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services;

public interface IOtpService
{
    /// <summary>
    /// Generates a One-Time Passcode (OTP) for a <see cref="Candidate"/>.
    /// </summary>
    /// <param name="candidate">The candidate receiving an OTP.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable task containing the resulting OTP.</returns>
    Task<string> GenerateOtpAsync(Candidate candidate, CancellationToken cancellationToken);

    /// <summary>
    /// Validates a One-Time Passcode (OTP) for a <see cref="Candidate"/>.
    /// </summary>
    /// <param name="mobilePhoneNumber">The candidate's mobile phone number, to which an OTP was sent.</param>
    /// <param name="otpChallenge">The OTP challenge.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable task containing the authenticated candidate.</returns>
    Task<Candidate?> ValidateOtpTokenAsync(string mobilePhoneNumber, string otpChallenge,
        CancellationToken cancellationToken);
}