using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Organization;

public class JobTitle : EntityBase<int>
{
    [MaxLength(50)]
    [Required]
    public required string Name { get; set; }
    
    public required int CompanyId { get; set; }
    
    public Company Company { get; set; }
    
    public ICollection<JobTitleAtsMapping> JobTitleAtsMappings { get; set; }
}