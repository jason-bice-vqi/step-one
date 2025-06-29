using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Serilog;
using ViaQuestInc.StepOne.Core.Auth;

namespace ViaQuestInc.StepOne.Web.Auth.RequirementsAndHandlers;

/// <summary>
/// Checks to see if the user is an administrator. If so, all requirements attached to the
/// <see cref="AuthorizationHandlerContext"/> are succeeded.
/// </summary>
public class AdministratorRequirement : IAuthorizationRequirement
{
}

public class AdministratorHandler : AuthorizationHandler<AdministratorRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministratorRequirement requirement)
    {
        Log.Verbose("Evaluating {Requirement}", nameof(AdministratorRequirement));

        // This requirement needs to be succeeded regardless so that non-admins don't fail on this requirement.
        context.Succeed(requirement);

        if (!context.User.IsInternal()) return Task.CompletedTask;

        Log.Verbose("  {User} is administrator. Succeeding all requirements:", context.User.GetNameIdentifierId());

        // This is where the real work is done: If the user is an admin, all requirements are succeeded.
        var otherRequirements = context.Requirements;

        foreach (var otherRequirement in otherRequirements)
        {
            Log.Verbose("    Succeeding {Requirement}", otherRequirement.GetType().Name);

            context.Succeed(otherRequirement);
        }

        return Task.CompletedTask;
    }
}