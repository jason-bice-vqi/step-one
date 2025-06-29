namespace ViaQuestInc.StepOne.Core.Workflows.Steps;

/// <summary>
/// The type of task a <see cref="Step"/> represents at its most basic level.
/// </summary>
public enum StepTypes
{
    /// <summary>
    /// A task that's completed at an external web site.
    /// </summary>
    ExternalHttpTask = 0,

    /// <summary>
    /// A data entry task.
    /// </summary>
    DataEntry = 1,

    /// <summary>
    /// A file that must be submitted/uploaded.
    /// </summary>
    FileSubmission = 2
}