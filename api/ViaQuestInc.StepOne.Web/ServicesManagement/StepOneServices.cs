using ViaQuestInc.StepOne.Core.Candidates.Import;
using ViaQuestInc.StepOne.Core.Candidates.Import.Operations;
using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Core.Workflows.Services;
using ViaQuestInc.StepOne.Infrastructure.Services;

namespace ViaQuestInc.StepOne.Web.ServicesManagement;

/// <summary>
/// Container for all custom/application service registrations.
/// </summary>
public static class StepOneServices
{
    public static IServiceCollection AddStepOneServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            // General services
            .AddScoped<CandidateService>()
            .AddScoped<CandidateWorkflowService>()
            .AddScoped<ExcelService>()
            .AddScoped<WorkflowService>()

            // Candidate import operations (ordered)
            .AddScoped<CandidateImportEngine>()
            .AddScoped<ICandidateImportOperation, ValidateMobileNumberOperation>()
            .AddScoped<ICandidateImportOperation, InitializeCandidateEntityOperation>()
            .AddScoped<ICandidateImportOperation, AbortExistingCandidateOperation>()
            .AddScoped<ICandidateImportOperation, CreateCandidateOperation>();
    }
}