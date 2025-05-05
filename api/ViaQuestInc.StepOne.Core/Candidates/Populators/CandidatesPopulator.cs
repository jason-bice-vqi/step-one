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
                EntityStatus = EntityStatuses.Active,
                FirstName = "Jason",
                FullName = "Bice, Jason",
                HireDate = new(2012, 04, 29),
                ImportedAt = DateTime.UtcNow,
                JobId = 1,
                JobTitle = "Software Architect",
                LastName = "Bice",
                PaycorCandidateId = Guid.NewGuid().ToString().Replace("-", string.Empty),
                PhoneNumber = "6144588078",
                StartDate = null
            }
        };
        
        await repository.CreateRangeAsync(testCandidates, cancellationToken);
    }
}