using ViaQuestInc.StepOne.Web.StartupActions;
using ViaQuestInc.StepOne.Web.StartupActions.Data;

namespace ViaQuestInc.StepOne.Web.ServicesManagement;

public static class DataServices
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Ordered
        return services.AddTransient<IStartupAction, LogDatabaseStartupTypeAction>()
            .AddTransient<IStartupAction, DeleteDatabaseAction>()
            .AddTransient<IStartupAction, CreateDatabaseAction>()
            .AddTransient<IStartupAction, ApplyMigrationsAction>()
            .AddTransient<IStartupAction, PopulateDatabaseAction>()
            .AddTransient<IStartupAction, WarnOfPendingMigrationsAction>(); // Must be last AcesDbContext action
    }
}