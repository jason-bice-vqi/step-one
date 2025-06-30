using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows.Services;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Populators;

/// <summary>
/// Creates a test/sample candidate workflow using the first candidate and first workflow in the system.
/// </summary>
public class CandidateWorkflowPopulator : IDataPopulator
{
    public async Task PopulateAsync(
        IRepository<StepOneDbContext> repository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken)
    {
        var candidate = await repository.All<Candidate>()
            .SingleAsync(cancellationToken);

        using var scope = serviceProvider.CreateScope();
        var workflowService = scope.ServiceProvider.GetRequiredService<WorkflowService>();
        var candidateWorkflowService = scope.ServiceProvider.GetRequiredService<CandidateWorkflowService>();
        var workflow = await workflowService.ShowAsync(1, cancellationToken);

        await candidateWorkflowService.CreateAsync(candidate, workflow, cancellationToken);
    }
}