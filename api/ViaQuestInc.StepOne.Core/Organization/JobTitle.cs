using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows;

namespace ViaQuestInc.StepOne.Core.Organization;

public class JobTitle : EntityBase<int>,
    IHrtDerivedEntity
{
    [MaxLength(100)]
    [Required]
    public required string Title { get; set; }
    
    [MaxLength(100)]
    public required string DisplayTitle { get; set; }

    public required int CompanyId { get; set; }

    public Company? Company { get; set; }
    
    public required Guid HrtId { get; set; }
    
    public int? WorkflowId { get; set; }
    
    public Workflow? Workflow { get; set; }
    
    public required EntityStatuses EntityStatus { get; set; }

    public ICollection<JobTitleAlias> JobTitleAliases { get; set; }

    public string DisplayTitleWithAbbr =>
        Company == null
            ? DisplayTitle
            : $"({Company.Abbreviation}) {DisplayTitle}";
}