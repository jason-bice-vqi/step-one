using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Serilog;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Candidates.Workflows;

namespace ViaQuestInc.StepOne.Web.Auth.RequirementsAndHandlers;

/// <summary>
/// Stipulates that the requester and resource share a common agent.
/// </summary>
public class CandidateWorkflowBelongsToRequesterRequirement : IAuthorizationRequirement
{
}

public class
    CandidateWorkflowBelongsToRequesterHandler
    : AuthorizationHandler<CandidateWorkflowBelongsToRequesterRequirement,
        CandidateWorkflow>
{
    private const string FailureReason = $"User does not own the requested {nameof(CandidateWorkflow)}.";

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CandidateWorkflowBelongsToRequesterRequirement requirement,
        CandidateWorkflow workflow)
    {
        Log.Verbose("Evaluating {Requirement}", nameof(CandidateWorkflowBelongsToRequesterRequirement));

        ArgumentNullException.ThrowIfNull(workflow);

        if (context.User.GetCandidateId() == workflow.CandidateId)
        {
            Log.Verbose("  GRANTED: Access to candidate workflow for user {User}.", context.User.GetNameIdentifierId());

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        Log.Warning("  DENIED: Access to candidate workflow for user {User}.", context.User.GetNameIdentifierId());

        context.Fail(new(this, FailureReason));

        return Task.CompletedTask;
    }
}