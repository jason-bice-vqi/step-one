using Microsoft.Identity.Client;

namespace ViaQuestInc.StepOne.Core.Auth.AzureActiveDirectory.Services;

public class AzureAdAuthService(AuthConfig authConfig)
{
    public async Task<string?> AuthenticateUserAsync(CancellationToken cancellationToken)
    {
        var app = PublicClientApplicationBuilder.Create(authConfig.AzureAd.ClientId)
            //.WithAuthority(new Uri($"https://login.microsoftonline.com/{authConfig.AzureAd.TenantId}"))
            .WithAuthority(new Uri($"{authConfig.AzureAd.Instance}/{authConfig.AzureAd.TenantId}"))
            .WithRedirectUri("http://localhost:4200/intermediate")
            .Build();

        try
        {
            var result = await app.AcquireTokenInteractive(new[] { "User.Read" })
                .WithPrompt(Prompt.SelectAccount)
                .ExecuteAsync(cancellationToken);

            return result.AccessToken;
        }
        catch (MsalUiRequiredException)
        {
            return "User authentication failed.";
        }
    }
}