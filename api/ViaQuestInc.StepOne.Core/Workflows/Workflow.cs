using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Workflows;

public class Workflow : EntityBase<int>
{
    /// <summary>
    /// The name of this workflow, if desired to override the default name derived from the in-scope job title.
    /// </summary>
    [MaxLength(100)]
    public required string? Name { get; set; }

    /// <summary>
    /// The workflow from which this workflow was copied, if this was a copied workflow. Used to copy navigation
    /// properties on inbound requests.
    /// </summary>
    [NotMapped]
    public int? CopiedFromWorkflowId { get; set; }

    [JsonIgnore]
    public Workflow? CopiedFromWorkflow { get; set; }

    /// <summary>
    /// Whether the job assignments should be copied to this workflow when copied from <see cref="CopiedFromWorkflow"/>.
    /// </summary>
    [NotMapped]
    public bool CopyJobAssignments { get; set; }

    /// <summary>
    /// Whether the steps should be copied to this workflow when copied from <see cref="CopiedFromWorkflow"/>.
    /// </summary>
    [NotMapped]
    public bool CopySteps { get; set; }

    public ICollection<WorkflowStep>? WorkflowSteps { get; set; }
}