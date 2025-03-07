using ViaQuestInc.StepOne.Core.Candidates;

namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services;

public interface IOtpService
{
    /// <summary>
    /// Sends a One-Time Passcode (OTP) for a <see cref="Candidate"/>.
    /// </summary>
    /// <param name="phoneNumber">The candidate's phone number.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable task indicating whether the OTP was sent. If it was not sent, it's because the supplied
    /// phone number does not belong to a matching <see cref="Candidate"/> record.</returns>
    Task<bool> SendOtpAsync(string phoneNumber, CancellationToken cancellationToken);

    /// <summary>
    /// Validates a One-Time Passcode (OTP) for a <see cref="Candidate"/>.
    /// </summary>
    /// <param name="phoneNumber">The candidate's mobile phone number, to which an OTP was sent.</param>
    /// <param name="otp">The OTP challenge.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable task containing a JWT. If validation failed, this will be null.</returns>
    Task<string?> ValidateOtpTokenAsync(string phoneNumber, string otp, CancellationToken cancellationToken);
}