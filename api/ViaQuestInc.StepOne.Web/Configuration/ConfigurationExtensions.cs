namespace ViaQuestInc.StepOne.Web.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddStepOneConfigurations(this IServiceCollection services,
        IWebHostEnvironment env,
        IConfiguration configuration)
    {
        return services.Configure<ServerConfig>(configuration.GetSection("Server"));
    }
}