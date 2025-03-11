using Serilog;
using ViaQuestInc.StepOne.Web.ServiceModules.Auth;

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

        Log.Information("");
        
        builder.AddModule<AuthModule>("Auth")
            .AddModule<DatabaseModule>("Database");

        return services;
    }
}