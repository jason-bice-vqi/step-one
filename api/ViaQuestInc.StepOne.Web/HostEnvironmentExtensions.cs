namespace ViaQuestInc.StepOne.Web;

/// <summary>
/// Extension methods for <see cref="IHostEnvironment"/>.
/// </summary>
public static class HostEnvironmentExtensions
{
    private const string Local = "Local";

    /// <summary>
    /// Checks if the current host environment name is <see cref="Local"/>.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
    /// <returns>True if the environment name is <see cref="Local"/>.</returns>
    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null) throw new ArgumentNullException(nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Local);
    }

    /// <summary>
    /// Checks if the current host environment name is <see cref="Local"/>
    /// or <see cref="EnvironmentName.Development"/>.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
    /// <returns>True if the environment name is <see cref="Local"/> or
    /// <see cref="EnvironmentName.Development"/>, otherwise false.</returns>
    public static bool IsLocalOrDevelopment(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null) throw new ArgumentNullException(nameof(hostEnvironment));

        return hostEnvironment.IsEnvironment(Local) || hostEnvironment.IsEnvironment(Environments.Development);
    }
}