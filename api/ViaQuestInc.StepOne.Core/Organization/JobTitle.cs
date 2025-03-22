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
    
    public int? WorkflowId { get; set; }
    
    /// <summary>
    /// The onboarding workflow assigned to this job title. If no workflow has been assigned
    /// (see null status of <see cref="WorkflowId"/>), this will be null.
    /// </summary>
    public Workflow? Workflow { get; set; }
}