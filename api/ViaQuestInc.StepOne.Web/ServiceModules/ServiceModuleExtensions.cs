namespace ViaQuestInc.StepOne.Web.ServiceModules;

/// <summary>
/// Container for all custom/application module registrations.
/// </summary>
public static class ServiceModuleExtensions
{
    public static IServiceCollection AddStepOneModules(this IServiceCollection services,
        IWebHostEnvironment env,
        IConfiguration configuration)
    {
        var builder = new ServiceModulesBuilder(services, env, configuration);
        
        builder.AddModule<DatabaseModule>("Database");

        return services;
    }
}