using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class AuthModule : IServiceModule
{
    public const string OtpScheme = nameof(OtpScheme);
    public const string InternalUserPolicy = nameof(InternalUserPolicy);
    public const string ExternalUserPolicy = nameof(ExternalUserPolicy);

    public void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<IOtpService, OtpService>();

        // VQI users (Entra ID)
        services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddMicrosoftIdentityWebApi(configuration.GetSection("Auth:AzureAd"));

        // Candidate users (OTP)
        services.AddAuthentication(OtpScheme) // OTP for External Users
            .AddScheme<AuthenticationSchemeOptions, OtpAuthenticationHandler>(OtpScheme, null);

        // Policies
        services.AddAuthorizationBuilder()
            .AddPolicy(InternalUserPolicy, x => x.RequireAuthenticatedUser().RequireRole(InternalUserPolicy))
            .AddPolicy(ExternalUserPolicy, x => x.RequireAuthenticatedUser().RequireRole(ExternalUserPolicy));
    }
}