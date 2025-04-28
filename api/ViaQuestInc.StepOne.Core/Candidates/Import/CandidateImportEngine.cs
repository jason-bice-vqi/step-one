using System.Data;
using ViaQuestInc.StepOne.Core.Candidates.Import.Operations;

namespace ViaQuestInc.StepOne.Core.Candidates.Import;

public class CandidateImportEngine(IEnumerable<ICandidateImportOperation> candidateImportOperations)
{
    public async Task<ICollection<CandidateImportResult>> ImportAsync(
        EnumerableRowCollection<DataRow> rawCandidateDataRows,
        CancellationToken cancellationToken)
    {
        var options = new CandidateImportOptions();

        foreach (var rawCandidateDataRow in rawCandidateDataRows)
        {
            options.CurrentRawCandidateDataRow = rawCandidateDataRow;
            
            if (options.Abort)
            {
                options.Abort = false;

                continue;
            }

            foreach (var candidateImportOperation in candidateImportOperations)
            {
                if (options.Abort || !await candidateImportOperation.ShouldExecuteAsync(options, cancellationToken))
                {
                    continue;
                }

                await candidateImportOperation.ExecuteAsync(options, cancellationToken);
            }

            // Reset for the next iteration; must be last.
            options.CurrentRawCandidateDataRow = null;
            options.InitializedCandidateEntity = null;
        }

        return null; // todo
    }
}