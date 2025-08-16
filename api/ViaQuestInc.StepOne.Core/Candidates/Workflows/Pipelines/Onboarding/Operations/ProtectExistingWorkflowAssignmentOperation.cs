using Serilog;
using ViaQuestInc.StepOne.Core.Kernel;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Workflows;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;

/// <summary>
/// An operation that ensures a <see cref="JobTitle"/>'s existing <see cref="Workflow"/> assignment isn't permanently
/// reassigned by the request. The UI should not allow this to occur, so if the request comes in with an altered
/// assignment and a request to make that assignment permanent, the payload is no longer trustworthy. This operation
/// will log an error and then throw an exception
/// </summary>
public class ProtectExistingWorkflowAssignmentOperation : ICandidateOnboardingOperation
{
    public Task<bool> ShouldExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        if (!options.Request.AssignWorkflowToJobTitle || options.Request.JobTitle!.WorkflowId == null)
        {
            return Task.FromResult(false);
        }

        var workflowAssignmentAltered = options.Request.JobTitle!.WorkflowId != options.Request.JobTitle.WorkflowId;
        
        return Task.FromResult(workflowAssignmentAltered);
    }

    public Task ExecuteAsync(CandidateOnboardingOptions options, CancellationToken cancellationToken)
    {
        Log.Error(
            "Permanent workflow assignment requested for job title with existing workflow assignment.{newline}{request}",
            Environment.NewLine,
            options.Request);
        
        throw new IllegalRequestPayloadException();
    }
}