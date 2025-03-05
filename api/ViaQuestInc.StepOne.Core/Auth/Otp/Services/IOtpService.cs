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

    Task<bool> EmailOtpAsync(string email, string otp, CancellationToken cancellationToken);

    Task<bool> SmsOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken);
    
    Task<bool> SendOtpAsync(string emailOrPhone, string otp, CancellationToken cancellationToken);

    /// <summary>
    /// Validates a One-Time Passcode (OTP) for a <see cref="Candidate"/>.
    /// </summary>
    /// <param name="phoneNumber">The candidate's mobile phone number, to which an OTP was sent.</param>
    /// <param name="otp">The OTP challenge.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable task containing the authenticated candidate.</returns>
    Task<Candidate?> ValidateOtpTokenAsync(string phoneNumber, string otp, CancellationToken cancellationToken);
}