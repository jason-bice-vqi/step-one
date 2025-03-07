using Serilog;
using Twilio;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Auth.AzureActiveDirectory.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;
using ViaQuestInc.StepOne.Core.Auth.Services;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class AuthModule : IServiceModule
{
    public const string OtpScheme = nameof(OtpScheme);
    public const string InternalUserPolicy = nameof(InternalUserPolicy);
    public const string ExternalUserPolicy = nameof(ExternalUserPolicy);

    public void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env)
    {
        Log.Information("  Registering auth config");
        var authConfig = configuration.GetSection("Auth").Get<AuthConfig>() ??
                         throw new InvalidOperationException("Auth configuration is missing");

        services.AddSingleton(authConfig);
        
        Log.Information("  Registering auth services");
        services.AddScoped<JwtService>();
        services.AddScoped<AzureAdAuthService>();
        services.AddScoped<IOtpService, TwilioService>();
        
        Log.Information("  Initializing TwilioClient");
        TwilioClient.Init(authConfig.Twilio.AccountSid, authConfig.Twilio.AuthToken);

        // VQI users (Entra ID)
        // services.AddAuthentication(x =>
        //     {
        //         x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     })
        //     .AddMicrosoftIdentityWebApi(configuration.GetSection("Auth:AzureAd"));

        // Candidate users (OTP)
        // services.AddAuthentication(OtpScheme) // OTP for External Users
        //     .AddScheme<AuthenticationSchemeOptions, OtpAuthenticationHandler>(OtpScheme, null);

        // Policies
        // services.AddAuthorizationBuilder()
        //     .AddPolicy(InternalUserPolicy, x => x.RequireAuthenticatedUser().RequireRole(InternalUserPolicy))
        //     .AddPolicy(ExternalUserPolicy, x => x.RequireAuthenticatedUser().RequireRole(ExternalUserPolicy));
    }
}