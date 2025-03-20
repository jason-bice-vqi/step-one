using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateService(IRepository repository)
{
    public async Task<Candidate?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<Candidate>(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task RecordAuthenticatedAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        candidate.LastAuthenticatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
    }

    public async Task RecordOtpRequestedAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        candidate.OtpLastRequestedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
    }
}