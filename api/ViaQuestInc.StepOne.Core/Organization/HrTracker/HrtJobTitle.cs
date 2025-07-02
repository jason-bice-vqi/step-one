using System.ComponentModel.DataAnnotations.Schema;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.HrTracker;

public class HrtJobTitle : EntityBase<Guid>,
    IHrtSourceEntity<JobTitle, int>
{
    [Column("JobTitleID")]
    public override required Guid Id { get; set; }

    [Column("CompanyID")]
    public Guid CompanyId { get; set; }

    public string? DisplayTitle { get; set; }

    public string Title { get; set; }

    [Column("Status")]
    public EntityStatuses EntityStatus { get; set; }

    public async Task<JobTitle> ToStepOneEntityAsync(
        IRepository<StepOneDbContext> repository,
        CancellationToken cancellationToken)
    {
        var company = await repository.GetAsync<Company>(x => x.HrtId == CompanyId, cancellationToken);

        if (company == null)
        {
            throw new(
                $"Cannot establish job title {Title} with HRT ID {Id} because its company {CompanyId} does not exist in the StepOne database.");
        }

        var jobTitle = new JobTitle
        {
            Id = default,
            Title = Title,
            DisplayTitle = DisplayTitle ?? Title,
            CompanyId = company.Id,
            HrtId = Id,
            EntityStatus = EntityStatus
        };

        return jobTitle;
    }
}