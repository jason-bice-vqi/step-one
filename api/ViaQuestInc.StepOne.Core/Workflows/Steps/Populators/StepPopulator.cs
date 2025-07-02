using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows.Steps.Populators;

public class StepPopulator : IDataPopulator
{
    public async Task PopulateAsync(
        IRepository<StepOneDbContext> repository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken)
    {
        var steps = new[]
        {
            // File Submissions
            new Step
            {
                Id = default,
                Descriptor = "Pre-Employment Form",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Complete Pre-Employment Form",
                StepType = StepTypes.FileSubmission
            },
            new Step
            {
                Id = default,
                Descriptor = "Driver's License",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Driver's License",
                StepType = StepTypes.FileSubmission
            },
            new Step
            {
                Id = default,
                Descriptor = "Transcripts or Diploma",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Proof of Highest Form of Education",
                StepType = StepTypes.FileSubmission
            },
            new Step
            {
                Id = default,
                Descriptor = "Offer Letter",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed Offer Letter",
                StepType = StepTypes.FileSubmission
            },
            new Step
            {
                Id = default,
                Descriptor = "Verification of Compliance Form",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed Verification of Compliance Form",
                StepType = StepTypes.FileSubmission
            },
            new Step
            {
                Id = default,
                Descriptor = "W9",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed W9",
                StepType = StepTypes.FileSubmission
            },
            new Step
            {
                Id = default,
                Descriptor = "Social Security Card",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Social Security Card",
                StepType = StepTypes.FileSubmission
            },

            // External Tasks
            new Step
            {
                Id = default,
                Descriptor = "Employment Application",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Complete Employment Application",
                StepType = StepTypes.ExternalHttpTask,
                ExternalHttpTaskLink = "https://www.viaquestinc.com" // TODO ATS link
            }
        };

        await repository.CreateRangeAsync(steps, cancellationToken);
    }
}