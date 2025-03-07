using Serilog;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class ServiceModulesBuilder(
    IServiceCollection services,
    IWebHostEnvironment environment,
    IConfiguration configuration)
{
    public IServiceCollection Services { get; } = services;

    public IWebHostEnvironment Environment { get; } = environment;

    public IConfiguration Configuration { get; } = configuration;

    /// <summary>
    /// Adds a service module to the services collection.
    /// </summary>
    /// <param name="configSectionName">The name of the configuration section to bind to or null if the module does
    /// not have configuration.</param>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    public ServiceModulesBuilder AddModule<TModule>(string configSectionName = null)
        where TModule : class, IServiceModule, new()
    {
        var module = new TModule();

        if (!string.IsNullOrEmpty(configSectionName))
        {
            Configuration.GetSection(configSectionName).Bind(module);
        }
        
        Log.Information("Loading {Module} module:", module.GetType().Name);
        
        module.Configure(Configuration, Services, Environment);
        
        Log.Information("");
        
        return this;
    }
}