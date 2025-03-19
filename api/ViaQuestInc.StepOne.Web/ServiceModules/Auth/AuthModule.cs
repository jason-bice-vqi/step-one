using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Twilio;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;
using ViaQuestInc.StepOne.Core.Auth.Services;

namespace ViaQuestInc.StepOne.Web.ServiceModules.Auth;

public class AuthModule : IServiceModule
{
    public const string OtpScheme = nameof(OtpScheme);

    public void Configure(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment env)
    {
        Log.Information("  Registering authentication services");
        services.AddScoped<JwtService>();
        services.AddScoped<IOtpService, TwilioService>();

        Log.Information("  Registering authentication config");
        var authConfig = configuration.GetSection("Auth").Get<AuthConfig>() ??
                         throw new InvalidOperationException("Auth configuration is missing");

        services.AddSingleton(authConfig);

        Log.Information("  Registering Azure AD JWT authentication for VQI (Entra) users");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(AuthSchemes.InitialAzureAdJwtAuthScheme, options =>
            {
                options.Authority = authConfig.AzureAd.Authority;
                options.Audience = authConfig.AzureAd.Audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://login.microsoftonline.com/{authConfig.AzureAd.TenantId}/v2.0",
                    ValidateAudience = true,
                    ValidAudience = authConfig.AzureAd.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = _ =>
                    {
                        Log.Information("Azure AD token validation PASSED.");
                        
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = v =>
                    {
                        Log.Error("Azure AD token validation FAILED: {Message} {Stack}", v.Exception.Message,
                            v.Exception.StackTrace);
                        
                        return Task.CompletedTask;
                    }
                };
            });

        Log.Information("  Registering JWT authentication policies");
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicies.InitialAzureAdJwtAuthPolicy, policy =>
                policy.RequireAuthenticatedUser().AddAuthenticationSchemes(AuthSchemes.InitialAzureAdJwtAuthScheme))
            .AddPolicy(AuthPolicies.DefaultJwtAuthPolicy, policy =>
                policy.RequireAuthenticatedUser().AddAuthenticationSchemes(AuthSchemes.DefaultJwtAuthScheme));

        Log.Information("  Registering role authorization policies");
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicies.InternalUserPolicy,
                x => x.RequireAuthenticatedUser().RequireRole(AuthRoles.Internal))
            .AddPolicy(AuthPolicies.ExternalUserPolicy,
                x => x.RequireAuthenticatedUser().RequireRole(AuthRoles.External));

        Log.Information("  Initializing TwilioClient");
        TwilioClient.Init(authConfig.Twilio.AccountSid, authConfig.Twilio.AuthToken);
    }
}