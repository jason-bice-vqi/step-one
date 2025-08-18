using Humanizer;
using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Kernel;

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
            throw new ArgumentNullException(nameof(options.CurrentRawCandidateDataRow));

        var firstName = options.CurrentRawCandidateDataRow["Candidate First Name"]
            .ToString()!.ToLower()
            .Titleize();
        var lastName = options.CurrentRawCandidateDataRow["Candidate Last Name"]
            .ToString()!.ToLower()
            .Titleize();

        options.InitializedCandidateEntity = new()
        {
            Id = 0,
            AddressLine1 = options.CurrentRawCandidateDataRow["Address 1:"]
                .ToString()
                .NullifyEmptyOrWhitespace()
                ?.ToLower()
                .Titleize(),
            AddressLine2 = options.CurrentRawCandidateDataRow["Address 2:"]
                .ToString()
                .NullifyEmptyOrWhitespace()
                ?.ToLower()
                .Titleize(),
            CandidateWorkflowStatus = CandidateWorkflowStatus.Unassigned,
            City = options.CurrentRawCandidateDataRow["City"]
                .ToString()
                .NullifyEmptyOrWhitespace()
                .OnlyLetters()
                ?
                .ToLower()
                .Titleize(),
            EntityStatus = EntityStatuses.Active,
            FirstName = firstName,
            FullName = $"{lastName}, {firstName}",
            ImportedAt = DateTime.UtcNow,
            LastName = lastName,
            HireDate = DateOnly.Parse(
                options.CurrentRawCandidateDataRow["Date Hired"]
                    .ToString()!),
            AtsJobId = int.Parse(
                options.CurrentRawCandidateDataRow["Job ID"]
                    .ToString()!),
            AtsJobTitle = options.CurrentRawCandidateDataRow["Job Title"]
                .ToString()!.ToUpper(),
            PaycorCandidateId = options.CurrentRawCandidateDataRow["Candidate ID"]
                .ToString()!,
            PhoneNumber = options.CurrentRawCandidateDataRow["Mobile Phone"]
                .ToString()!.ToUnformattedPhoneNumber()!,
            PostalCode = options.CurrentRawCandidateDataRow["Postal Code"]
                .ToString()
                .NullifyEmptyOrWhitespace(),
            StartDate = DateOnly.TryParse(
                options.CurrentRawCandidateDataRow["Start Date"]
                    .ToString()!,
                out var startDate)
                ? startDate
                : null,
            State = options.CurrentRawCandidateDataRow["State"]
                .ToString()
                .NullifyEmptyOrWhitespace()
        };

        if (options.InitializedCandidateEntity.State?.Length > 2)
            options.InitializedCandidateEntity.State = options.InitializedCandidateEntity.State.Titleize();

        return Task.CompletedTask;
    }
}