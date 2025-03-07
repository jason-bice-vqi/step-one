using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates.Populators;

public class CandidatesPopulator : IDataPopulator
{
    public async Task PopulateAsync(IRepository repository, IServiceProvider serviceProvider, int batchSize,
        CancellationToken cancellationToken)
    {
        var testCandidates = new List<Candidate>
        {
            new()
            {
                FirstName = "Jason",
                LastName = "Bice",
                PhoneNumber = "6144588078",
                EntityStatus = EntityStatuses.Active
            }
        };

        await repository.CreateRangeAsync(testCandidates, cancellationToken);
    }
}