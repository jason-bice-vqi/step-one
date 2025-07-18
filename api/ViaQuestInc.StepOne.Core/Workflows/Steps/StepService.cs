﻿using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Workflows.Steps;

public class StepService(IRepository<StepOneDbContext> repository)
{
    public async Task<Step> CreateAsync(Step step, CancellationToken cancellationToken)
    {
        return await repository.CreateAsync(step, cancellationToken);
    }

    public async Task<IEnumerable<Step>> IndexAsync(CancellationToken cancellationToken)
    {
        return await repository.All<Step>()
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Step?> ShowAsync(int stepId, CancellationToken cancellationToken)
    {
        return await repository.FindAsync<Step>(stepId, cancellationToken);
    }

    public async Task<int> UpdateAsync(Step step, CancellationToken cancellationToken)
    {
        return await repository.UpdateAsync(step, cancellationToken);
    }

    public async Task<int> DeleteAsync(Step step, CancellationToken cancellationToken)
    {
        return await repository.DeleteAsync(step, cancellationToken);
    }
}