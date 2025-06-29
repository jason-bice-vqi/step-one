using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Organization;

namespace ViaQuestInc.StepOne.Core.Workflows;

/// <summary>
/// Relates a <see cref="JobTitle"/> and <see cref="Workflow"/>.
/// </summary>
public class JobTitleWorkflow : EntityBase<int>
{
    public required int JobTitleId { get; set; }

    /// <summary>
    /// The job title to which this relationship belongs.
    /// </summary>
    public JobTitle JobTitle { get; set; }

    public required int WorkflowId { get; set; }

    /// <summary>
    /// The workflow to which this relationship belongs.
    /// </summary>
    public Workflow Workflow { get; set; }
}