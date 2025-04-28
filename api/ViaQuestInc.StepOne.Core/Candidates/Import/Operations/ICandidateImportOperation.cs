namespace ViaQuestInc.StepOne.Core.Candidates.Import.Operations;

public interface ICandidateImportOperation
{
    Task<bool> ShouldExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken);

    Task ExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken);
}