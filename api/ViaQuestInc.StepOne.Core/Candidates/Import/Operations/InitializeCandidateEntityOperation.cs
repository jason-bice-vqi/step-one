using ViaQuestInc.StepOne.Kernel;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates.Import.Operations;

public class InitializeCandidateEntityOperation : ICandidateImportOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public Task ExecuteAsync(CandidateImportOptions options, CancellationToken cancellationToken)
    {
        if (options.CurrentRawCandidateDataRow == null)
        {
            throw new ArgumentNullException(nameof(options.CurrentRawCandidateDataRow));    
        }
        
        options.InitializedCandidateEntity = new()
        {
            EntityStatus = EntityStatuses.Active,
            FirstName = options.CurrentRawCandidateDataRow["Candidate First Name"].ToString()!,
            LastName = options.CurrentRawCandidateDataRow["Candidate Last Name"].ToString()!,
            HireDate = DateTime.Parse(options.CurrentRawCandidateDataRow["Date Hired"].ToString()!),
            JobId = int.Parse(options.CurrentRawCandidateDataRow["Job ID"].ToString()!),
            JobTitle = options.CurrentRawCandidateDataRow["Job Title"].ToString()!,
            PaycorCandidateId = options.CurrentRawCandidateDataRow["Candidate ID"].ToString()!,
            PhoneNumber = options.CurrentRawCandidateDataRow["Phone Number"].ToString()!.ToUnformattedPhoneNumber()!,
            StartDate = DateTime.TryParse(options.CurrentRawCandidateDataRow["Start Hired"].ToString()!,
                out var startDate)
                ? startDate
                : null
        };
        
        return Task.CompletedTask;
    }
}