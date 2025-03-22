using Microsoft.AspNetCore.Authorization;
using ViaQuestInc.StepOne.Kernel.Auth;
using ViaQuestInc.StepOne.Web.Auth.RequirementsAndHandlers;

namespace ViaQuestInc.StepOne.Web.Auth;

public static class Policies
{
    /// <summary>
    /// The authorization policy for requests bearing a ViaQuest Azure AD JWT.
    /// </summary>
    public const string AzureAdJwtAuthPolicy = nameof(AzureAdJwtAuthPolicy);

    /// <summary>
    /// The authorization policy for requests bearing a native StepOne JWT.
    /// </summary>
    public const string NativeJwtAuthPolicy = nameof(NativeJwtAuthPolicy);
    
    public const string InternalUserPolicy = nameof(InternalUserPolicy);
    
    public const string ExternalUserPolicy = nameof(ExternalUserPolicy);
    
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
            {
                ExternalUserPolicy, builder => builder.RequireRole(Roles.External)
            },
            {
                InternalUserPolicy, builder => builder.RequireRole(Roles.Internal)
            }
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