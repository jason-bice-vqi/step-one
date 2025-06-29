using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Workflows.Populators;

public class WorkflowPopulator : IDataPopulator
{
    public async Task PopulateAsync(
        IRepository<StepOneDbContext> repository,
        IServiceProvider serviceProvider,
        int batchSize,
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
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed Offer Letter").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Complete Employment Application").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed Verification of Compliance Form").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Complete Pre-Employment Form").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed W9").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Proof of Highest Form of Education").Id
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
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed Offer Letter").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Complete Employment Application").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed Verification of Compliance Form").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Complete Pre-Employment Form").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed W9").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Proof of Highest Form of Education").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Driver's License").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Social Security Card").Id
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
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed Offer Letter").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Complete Employment Application").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed Verification of Compliance Form").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Complete Pre-Employment Form").Id
                    },
                    new WorkflowStep
                    {
                        StepId = steps.Single(x => x.NameDefault == "Submit Signed W9").Id
                    }
                }
            }
        };

        foreach (var workflow in workflows)
        {
            var workflowSteps = workflow.WorkflowSteps.ToArray();

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