using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows.Steps.Populators;

public class StepPopulator : IDataPopulator
{
    public async Task PopulateAsync(IRepository repository, IServiceProvider serviceProvider, int batchSize,
        CancellationToken cancellationToken)
    {
        var steps = new[]
        {
            // File Submissions
            new Step
            {
                Descriptor = "Pre-Employment Form",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Complete Pre-Employment Form",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedPreEmployment
            },
            new Step
            {
                Descriptor = "Driver's License",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Driver's License",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SubmitDriversLicense
            },
            new Step
            {
                Descriptor = "Transcripts or Diploma",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Proof of Highest Form of Education",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.ProofOfHighestFormOfEducation
            },
            new Step
            {
                Descriptor = "Offer Letter",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed Offer Letter",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedOfferLetter
            },
            new Step
            {
                Descriptor = "Verification of Compliance Form",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed Verification of Compliance Form",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedVerificationOfCompliance
            },
            new Step
            {
                Descriptor = "W9",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed W9",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedW9
            },
            new Step
            {
                Descriptor = "Social Security Card",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Social Security Card",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SubmitSocialSecurityCard
            },
            
            // External Tasks
            new Step
            {
                Descriptor = "Employment Application",
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Complete Employment Application",
                StepType = StepTypes.ExternalHttpTask,
                ExternalHttpTaskLink = "https://www.viaquestinc.com", // TODO ATS link
                WorkflowStepType = WorkflowStepTypes.AtsEmploymentApplication
            }
        };

        await repository.CreateRangeAsync(steps, cancellationToken);
    }
}