using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Populators;

public class CompanyPopulator : IDataPopulator
{
    public async Task PopulateAsync(
        IRepository<StepOneDbContext> repository,
        IServiceProvider serviceProvider,
        int batchSize,
        CancellationToken cancellationToken)
    {
        var companies = new Company[]
        {
            new()
            {
                Name = "ViaQuest Day & Employment Services LLC",
                Abbreviation = "VDES",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("D1AAD9D1-2989-4E16-A6D1-146BF26C5947")
            },
            new()
            {
                Name = "ViaQuest Psychiatric & Behavioral Solutions LLC",
                Abbreviation = "VPBS",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("244830D3-FF9A-443D-BEEA-174DF6AAC17B")
            },
            new()
            {
                Name = "ViaQuest Hospice LLC",
                Abbreviation = "HSP OH & PA",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("4DE2CDA6-4C39-482F-9021-34F8E2BB8FAA")
            },
            new()
            {
                Name = "ViaQuest Healthcare North LLC",
                Abbreviation = "VHC",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("9CE113EE-2382-4FB8-9616-64DAC9EACB57")
            },
            new()
            {
                Name = "ViaQuest Foundation",
                Abbreviation = "VQF",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("9F18CA68-3341-41F4-912F-7992111C6A3E")
            },
            new()
            {
                Name = "ViaQuest Community Solutions LLC",
                Abbreviation = "VCS",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("9E652253-F85D-4194-8259-A1F3C3464263")
            },
            new()
            {
                Name = "ViaQuest LLC",
                Abbreviation = "VQI",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("720AD528-906D-4BC2-976E-AA4D17E14232")
            },
            new()
            {
                Name = "ViaQuest Hospice of Indiana LLC",
                Abbreviation = "HSP IN",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("C0915AF8-B587-4AE0-8105-CD8A1DC79986")
            },
            new()
            {
                Name = "ViaQuest Residential Services LLC",
                Abbreviation = "VRS",
                EntityStatus = EntityStatuses.Active,
                HrtId = Guid.Parse("A7D234B2-12BB-459E-87BE-EDC87D2F677C")
            }
        };

        await repository.CreateRangeAsync(companies, cancellationToken);
    }
}