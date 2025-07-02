using System.ComponentModel.DataAnnotations.Schema;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.HrTracker;

public class HrtCompany : EntityBase<Guid>,
    IHrtSourceEntity<Company, int>
{
    [Column("CompanyID")]
    public override required Guid Id { get; set; }

    [Column("CompanyName")]
    public string Name { get; set; }

    public string Abbreviation { get; set; }

    [Column("Status")]
    public EntityStatuses EntityStatus { get; set; }

    public Task<Company> ToStepOneEntityAsync(
        IRepository<StepOneDbContext> repository,
        CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Id = default,
            Name = Name,
            Abbreviation = Abbreviation,
            HrtId = Id,
            EntityStatus = EntityStatus
        };

        return Task.FromResult(company);
    }
}