using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Workflows.Steps;

/// <summary>
/// The fundamental definition of a task. Provides a default template for a <see cref="WorkflowStep"/>.
/// </summary>
public class Step : EntityBase<int>
{
    /// <summary>
    /// The type of step this entity represents.
    /// </summary>
    public required StepTypes StepType { get; set; }
    
    /// <summary>
    /// The specific nature of this workflow step.
    /// </summary>
    public required WorkflowStepTypes WorkflowStepType { get; set; }
    
    /// <summary>
    /// The default name of this step. Can be overridden by any <see cref="WorkflowStep"/> to which this step belongs.
    /// </summary>
    [MaxLength(100)]
    [Required]
    public required string NameDefault { get; set; }
    
    /// <summary>
    /// Whether an administrator is required to review/approve the completion of this step before it's considered done.
    /// Can be overridden by any <see cref="WorkflowStep"/> to which this step belongs.
    /// </summary>
    public bool IsAdminConfirmationRequiredDefault { get; set; }
    
    /// <summary>
    /// The link to an external site if <see cref="StepType"/> is <see cref="StepTypes.ExternalHttpTask"/>; null
    /// otherwise.
    /// </summary>
    [MaxLength(255)]
    public string? ExternalHttpTaskLink { get; set; }
}