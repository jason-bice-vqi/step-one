using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows;

public class Workflow : EntityBase<int>
{
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    
    public ICollection<WorkflowStep> WorkflowSteps { get; set; }
}