using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows.Steps;

/// <summary>
/// Relates a <see cref="Step"/> to a <see cref="Workflow"/>, providing workflow-specific customization and
/// configuration.
/// </summary>
public class WorkflowStep : EntityBase<int>
{
    public int WorkflowId { get; set; }
    
    /// <summary>
    /// The workflow with which this step is associated.
    /// </summary>
    public Workflow? Workflow { get; set; }
    
    public required int StepId { get; set; }
    
    /// <summary>
    /// The step with which this workflow is associated.
    /// </summary>
    public Step Step { get; set; }
    
    /// <summary>
    /// Whether an administrator is required to review/approve the completion of this step before it's considered done.
    /// </summary>
    public bool IsAdminConfirmationRequired { get; set; }
    
    /// <summary>
    /// An override, if desired, of <see cref="Step.NameDefault"/>.
    /// </summary>
    [MaxLength(100)]
    public string? StepNameOverride { get; set; }
    
    /// <summary>
    /// The display order of this step within the workflow.
    /// </summary>
    public int StepIndex { get; set; }
}