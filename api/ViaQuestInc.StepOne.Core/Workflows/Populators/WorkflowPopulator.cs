using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Workflows.Steps;
using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows.Populators;

public class WorkflowPopulator : IDataPopulator
{
    public async Task PopulateAsync(IRepository repository, IServiceProvider serviceProvider, int batchSize,
        CancellationToken cancellationToken)
    {
        var steps = await repository.All<Step>().ToArrayAsync(cancellationToken);

        // Arrange steps in display order, as StepIndex will be established based on this order.
        var workflows = new[]
        {
            new Workflow
            {
                Name = "PBS w/ Credentialing (Non-Nursing)",
                WorkflowSteps = new[]
                {
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedOfferLetter)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedOfferLetter).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.AtsEmploymentApplication)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.AtsEmploymentApplication).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedVerificationOfCompliance)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x =>
                            x.WorkflowStepType == WorkflowStepTypes.SignedVerificationOfCompliance).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedPreEmployment)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedPreEmployment).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedW9)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedW9).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.ProofOfHighestFormOfEducation)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps
                            .Single(x => x.WorkflowStepType == WorkflowStepTypes.ProofOfHighestFormOfEducation).Id
                    }
                }
            },
            new Workflow
            {
                Name = "PBS w/ Credentialing (Nursing)",
                WorkflowSteps = new[]
                {
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedOfferLetter)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedOfferLetter).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.AtsEmploymentApplication)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.AtsEmploymentApplication).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedVerificationOfCompliance)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x =>
                            x.WorkflowStepType == WorkflowStepTypes.SignedVerificationOfCompliance).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedPreEmployment)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedPreEmployment).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedW9)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedW9).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.ProofOfHighestFormOfEducation)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps
                            .Single(x => x.WorkflowStepType == WorkflowStepTypes.ProofOfHighestFormOfEducation).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SubmitDriversLicense)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SubmitDriversLicense).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SubmitSocialSecurityCard)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SubmitSocialSecurityCard).Id
                    }
                }
            },
            new Workflow
            {
                Name = "PBS (no Credentialing)",
                WorkflowSteps = new[]
                {
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedOfferLetter)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedOfferLetter).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.AtsEmploymentApplication)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.AtsEmploymentApplication).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedVerificationOfCompliance)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x =>
                            x.WorkflowStepType == WorkflowStepTypes.SignedVerificationOfCompliance).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedPreEmployment)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedPreEmployment).Id
                    },
                    new WorkflowStep
                    {
                        IsAdminConfirmationRequired =
                            steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedW9)
                                .IsAdminConfirmationRequiredDefault,
                        StepId = steps.Single(x => x.WorkflowStepType == WorkflowStepTypes.SignedW9).Id
                    },
                }
            }
        };

        foreach (var workflow in workflows)
        {
            var workflowSteps = workflow.WorkflowSteps!.ToArray();

            await repository.CreateAsync(workflow, cancellationToken);

            var stepIndex = 0;

            foreach (var workflowStep in workflowSteps)
            {
                workflowStep.StepIndex = stepIndex++;
                workflowStep.WorkflowId = workflow.Id;
            }

            await repository.CreateRangeAsync(workflowSteps, cancellationToken);
        }
    }
}