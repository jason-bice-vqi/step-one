using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ViaQuestInc.StepOne.Core.Data.Entity;

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
    [JsonIgnore]
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
    /// The step name, with <see cref="StepNameOverride"/> considered.
    /// </summary>
    public string StepName => StepNameOverride ?? Step.NameDefault;

    /// <summary>
    /// The display order of this step within the workflow.
    /// </summary>
    public int StepIndex { get; set; }

    /// <summary>
    /// Whether this step should block downstream steps until it is completed.
    /// </summary>
    public bool BlockDownstream { get; set; }

    /// <summary>
    /// Whether this step requires administrator confirmation to be considered completed.
    /// </summary>
    // TODO - get rid of this; dupe
    public bool RequiresAdminConfirmation { get; set; }
}