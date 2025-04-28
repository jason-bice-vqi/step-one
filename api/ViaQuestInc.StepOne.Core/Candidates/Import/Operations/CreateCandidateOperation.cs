using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Import.Operations;

public class CreateCandidateOperation(IRepository repository) : ICandidateImportOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public async Task ExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        await repository.CreateAsync(options.InitializedCandidateEntity!, cancellationToken);
    }
}