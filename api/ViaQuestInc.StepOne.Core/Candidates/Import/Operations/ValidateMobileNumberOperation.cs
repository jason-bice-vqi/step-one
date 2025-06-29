using ViaQuestInc.StepOne.Core.Kernel;

namespace ViaQuestInc.StepOne.Core.Candidates.Import.Operations;

public class ValidateMobileNumberOperation : ICandidateImportOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public Task ExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        var rawMobileNumber = options.CurrentRawCandidateDataRow["Mobile Phone"].ToString()?.ToUnformattedPhoneNumber();

        if (rawMobileNumber == null) options.Abort = true;

        return Task.CompletedTask;
    }
}