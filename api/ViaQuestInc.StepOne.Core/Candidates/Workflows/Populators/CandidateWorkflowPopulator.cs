using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Core.Workflows.Services;
using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Populators;

/// <summary>
/// Creates a test/sample candidate workflow using the first candidate and first workflow in the system.
/// </summary>
public class CandidateWorkflowPopulator : IDataPopulator
{
    public async Task PopulateAsync(IRepository repository, IServiceProvider serviceProvider, int batchSize,
        CancellationToken cancellationToken)
    {
        var candidate = await repository.All<Candidate>().SingleAsync(cancellationToken);
        
        using var scope = serviceProvider.CreateScope();
        var workflowService = scope.ServiceProvider.GetRequiredService<WorkflowService>();
        var candidateWorkflowService = scope.ServiceProvider.GetRequiredService<CandidateWorkflowService>();
        var workflow = await workflowService.GetAsync(1, cancellationToken);

        await candidateWorkflowService.CreateAsync(candidate, workflow, cancellationToken);
    }
}