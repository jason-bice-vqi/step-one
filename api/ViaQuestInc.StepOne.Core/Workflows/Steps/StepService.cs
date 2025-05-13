using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Steps;

public class StepService(IRepository repository)
{
    public async Task<int> CreateAsync(Step step, CancellationToken cancellationToken)
    {
        await repository.CreateAsync(step, cancellationToken);

        return step.Id;
    }

    public async Task<int> DeleteAsync(Step step, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(step, cancellationToken);
    }
    
    public async Task<IEnumerable<Step>> IndexAsync(CancellationToken cancellationToken)
    {
        return await repository.All<Step>().ToArrayAsync(cancellationToken);
    }

    public async Task<Step?> ShowAsync(int stepId, CancellationToken cancellationToken)
    {
        return await repository.FindAsync<Step>(stepId, cancellationToken);
    }
}