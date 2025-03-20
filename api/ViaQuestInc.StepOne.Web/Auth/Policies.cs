using Microsoft.AspNetCore.Authorization;
using ViaQuestInc.StepOne.Web.Auth.RequirementsAndHandlers;

namespace ViaQuestInc.StepOne.Web.Auth;

public static class Policies
{
    public static class CandidateWorkflows
    {
        public const string CanAccessCandidateWorkflow = nameof(CanAccessCandidateWorkflow);    
    }
    
    /// <summary>
    /// The comprehensive policy map used for endpoint authorization throughout the system.
    /// </summary>
    private static readonly Dictionary<string, Action<AuthorizationPolicyBuilder>> PolicyMap =
        new()
        {
            {
                CandidateWorkflows.CanAccessCandidateWorkflow, builder => builder
                    .AddRequirements(
                        new AdministratorRequirement(),
                        new CandidateWorkflowBelongsToRequesterRequirement()
                    )
            },
        };
    
    public static void SetupPolicies(AuthorizationOptions options)
    {
        foreach (var (key, value) in PolicyMap)
        {
            options.AddPolicy(
                key,
                policy =>
                {
                    value.Invoke(policy);
                }
            );
        }
    }
}