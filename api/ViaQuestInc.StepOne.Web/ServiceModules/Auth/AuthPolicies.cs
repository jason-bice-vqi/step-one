namespace ViaQuestInc.StepOne.Web.ServiceModules.Auth;

public static class AuthPolicies
{
    public const string InitialAzureAdJwtAuthPolicy = nameof(InitialAzureAdJwtAuthPolicy);

    public const string DefaultJwtAuthPolicy = nameof(DefaultJwtAuthPolicy);
    
    public const string InternalUserPolicy = nameof(InternalUserPolicy);
    
    public const string ExternalUserPolicy = nameof(ExternalUserPolicy);
}