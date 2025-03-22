using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Core.Workflows.Services;

namespace ViaQuestInc.StepOne.Web.ServicesManagement;

/// <summary>
/// Container for all custom/application service registrations.
/// </summary>
public static class StepOneServices
{
    public static IServiceCollection AddStepOneServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped<CandidateService>()
            .AddScoped<CandidateWorkflowService>()
            .AddScoped<WorkflowService>();
    }
}