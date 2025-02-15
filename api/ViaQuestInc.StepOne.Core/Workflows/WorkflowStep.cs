using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows;

public class WorkflowStep : EntityBase<int>
{
    public int WorkflowId { get; set; }
    
    public Workflow Workflow { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    
    public int StepIndex { get; set; }
}