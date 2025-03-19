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
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Complete Pre-Employment Form",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedPreEmployment
            },
            new Step
            {
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Driver's License",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SubmitDriversLicense
            },
            new Step
            {
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Proof of Highest Form of Education",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.ProofOfHighestFormOfEducation
            },
            new Step
            {
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed Offer Letter",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedOfferLetter
            },
            new Step
            {
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed Verification of Compliance Form",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedVerificationOfCompliance
            },
            new Step
            {
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Signed W9",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SignedW9
            },
            new Step
            {
                IsAdminConfirmationRequiredDefault = true,
                NameDefault = "Submit Social Security Card",
                StepType = StepTypes.FileSubmission,
                WorkflowStepType = WorkflowStepTypes.SubmitSocialSecurityCard
            },
            
            // External Tasks
            new Step
            {
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