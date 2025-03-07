using Microsoft.Identity.Client;

namespace ViaQuestInc.StepOne.Core.Auth.AzureActiveDirectory.Services;

public class AzureAdAuthService(AuthConfig authConfig)
{
    public async Task<string?> AuthenticateUserAsync(string username, string password,
        CancellationToken cancellationToken)
    {
        var app = PublicClientApplicationBuilder.Create(authConfig.AzureAd.ClientId)
            .WithAuthority($"{authConfig.AzureAd.Instance}{authConfig.AzureAd.TenantId}")
            .Build();

        try
        {
            var result = await app.AcquireTokenByUsernamePassword(
                new[] { "User.Read" },
                username,
                password
            ).ExecuteAsync(cancellationToken);
            
            // TODO - pull claims from AD token and create StepOne token with those claims and app claims.
            return result.AccessToken;
        }
        catch (MsalUiRequiredException)
        {
            return "User authentication failed.";
        }
    }
}