using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Import.Operations;

public class AbortExistingCandidateOperation(IRepository<StepOneDbContext> repository) : ICandidateImportOperation
{
    public async Task<bool> ShouldExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        var paycorCandidateId = options.InitializedCandidateEntity!.PaycorCandidateId;

        var existingCandidate =
            await repository.GetAsync<Candidate>(x => x.PaycorCandidateId == paycorCandidateId, cancellationToken);

        return existingCandidate is not null;
    }

    public Task ExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        options.Abort = true;

        return Task.CompletedTask;
    }
}