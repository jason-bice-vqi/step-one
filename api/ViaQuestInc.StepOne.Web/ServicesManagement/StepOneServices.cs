using ViaQuestInc.StepOne.Core.Candidates.Import;
using ViaQuestInc.StepOne.Core.Candidates.Import.Operations;
using ViaQuestInc.StepOne.Core.Candidates.Services;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Services;
using ViaQuestInc.StepOne.Core.Kernel.Services;
using ViaQuestInc.StepOne.Core.Organization.Services;
using ViaQuestInc.StepOne.Core.Workflows.Persistence;
using ViaQuestInc.StepOne.Core.Workflows.Persistence.Operations;
using ViaQuestInc.StepOne.Core.Workflows.Services;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

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
            .AddScoped<CompanyService>()
            .AddScoped<ExcelService>()
            .AddScoped<JobTitleAliasService>()
            .AddScoped<JobTitleService>()
            .AddScoped<StepService>()
            .AddScoped<WorkflowService>()

            // Candidate import operations (ordered)
            .AddScoped<CandidateImportEngine>()
            .AddScoped<ICandidateImportOperation, ValidateMobileNumberOperation>()
            .AddScoped<ICandidateImportOperation, InitializeCandidateEntityOperation>()
            .AddScoped<ICandidateImportOperation, AbortExistingCandidateOperation>()
            .AddScoped<ICandidateImportOperation, CreateCandidateOperation>()
            
            // Candidate onboarding operations (ordered)
            .AddScoped<CandidateOnboardingEngine>()
            .AddScoped<ICandidateOnboardingOperation, InitializeRequestOperation>()

            // Workflow persistence
            .AddScoped<WorkflowPersistenceEngine>()
            .AddScoped<IWorkflowPersistenceOperation, DeleteWorkflowStepsOperation>()
            .AddScoped<IWorkflowPersistenceOperation, ResequenceWorkflowStepsOperation>()
            .AddScoped<IWorkflowPersistenceOperation, PersistWorkflowStepsOperation>()
            .AddScoped<IWorkflowPersistenceOperation, PersistWorkflowOperation>();
    }
}