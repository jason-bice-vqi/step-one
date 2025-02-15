using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Web.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddStepOneConfigurations(this IServiceCollection services,
        IWebHostEnvironment env,
        IConfiguration configuration)
    {
        return services.Configure<DatabaseConfig>(configuration.GetSection("Database"))
            .Configure<ServerConfig>(configuration.GetSection("Server"));
    }
}