namespace ViaQuestInc.StepOne.Web.StartupActions;

/// <summary>
/// Encapsulates an action that is performed after the application starts up.
/// </summary>
public interface IStartupAction
{
    /// <summary>
    /// Indicates whether this startup action should execute.
    /// </summary>
    /// <returns>True if this action should execute; false otherwise.</returns>
    Task<bool> ShouldExecuteAsync(
        WebApplication webApplication,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Executes the startup action.
    /// </summary>
    Task OnStartupAsync(WebApplication webApplication, CancellationToken cancellationToken = default);
}