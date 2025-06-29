namespace ViaQuestInc.StepOne.Core.Auth;

public static class Schemes
{
    /// <summary>
    /// The scheme for authenticating/authorizing an incoming Azure AD JWT prior to exchanging for a native StepOne JWT.
    /// </summary>
    public const string InitialAzureAdJwtAuthScheme = nameof(InitialAzureAdJwtAuthScheme);

    /// <summary>
    /// The scheme for authenticating/authorizing application-native JWTs.
    /// </summary>
    public const string NativeJwtAuthScheme = nameof(NativeJwtAuthScheme);
}