namespace ViaQuestInc.StepOne.Web.ServiceModules;

/// <summary>
/// Encapsulates ASP.NET Core service configuration for a module.
/// </summary>
public interface IServiceModule
{
    /// <summary>
    /// Configures the provided services for this module.
    /// </summary>
    void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env);
}