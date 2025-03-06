using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Serilog;
using Twilio;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;
using ViaQuestInc.StepOne.Web.Auth;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class AuthModule : IServiceModule
{
    public const string OtpScheme = nameof(OtpScheme);
    public const string InternalUserPolicy = nameof(InternalUserPolicy);
    public const string ExternalUserPolicy = nameof(ExternalUserPolicy);

    public void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env)
    {
        const string twilioOtpConfigKey = "Twilio:Otp";
        
        services.Configure<TwilioOtpConfig>(configuration.GetSection(twilioOtpConfigKey));

        var twilioConfig = configuration.GetSection(twilioOtpConfigKey)!;
        
        Log.Information("Initializing TwilioClient...");
        
        TwilioClient.Init(twilioConfig["AccountSid"], twilioConfig["AuthToken"]);
        
        Log.Information("TwilioClient initialized.");
        
        services.AddScoped<IOtpService, TwilioOtpService>();

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