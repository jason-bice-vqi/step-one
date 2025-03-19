using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Core.Workflows.Steps;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows;

public class Workflow : EntityBase<int>
{
    /// <summary>
    /// The name of this workflow, if desired to override the default name derived from the in-scope job title.
    /// </summary>
    [MaxLength(100)]
    public required string? Name { get; set; }
    
    public ICollection<WorkflowStep>? WorkflowSteps { get; set; }
    
    public ICollection<JobTitleWorkflow> JobTitleWorkflows { get; set; }
}