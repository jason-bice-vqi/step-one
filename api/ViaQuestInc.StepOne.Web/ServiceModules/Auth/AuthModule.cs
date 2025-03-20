using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Twilio;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;
using ViaQuestInc.StepOne.Core.Auth.Services;
using ViaQuestInc.StepOne.Web.Auth;
using ViaQuestInc.StepOne.Web.Auth.RequirementsAndHandlers;

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

        Log.Information("  Registering JWT authentication schemes");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(AuthSchemes.InitialAzureAdJwtAuthScheme, options =>
            {
                options.Authority = authConfig.AzureAd.Authority;
                options.Audience = authConfig.AzureAd.Audience;

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://login.microsoftonline.com/{authConfig.AzureAd.TenantId}/v2.0",
                    ValidateAudience = true,
                    ValidAudience = authConfig.AzureAd.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                
                options.Events = new()
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
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Jwt.Key));
                
                options.Authority = authConfig.Jwt.Issuer;
                options.Audience = authConfig.Jwt.Audience;
                options.TokenValidationParameters = new()
                {
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = authConfig.Jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authConfig.Jwt.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                
                options.IncludeErrorDetails = true;
                
                options.Events = new()
                {
                    OnTokenValidated = _ =>
                    {
                        Log.Information("StepOne token validation PASSED.");
                        
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = v =>
                    {
                        Log.Error("StepOne token validation FAILED: {Message} {Stack}", v.Exception.Message,
                            v.Exception.StackTrace);
                        
                        switch (v.Exception)
                        {
                            case SecurityTokenExpiredException:
                                Log.Warning("Token is expired.");
                                break;
                            case SecurityTokenInvalidSignatureException:
                                Log.Warning("Token has an invalid signature.");
                                break;
                            case SecurityTokenInvalidIssuerException:
                                Log.Warning("Token has an invalid issuer.");
                                break;
                            case SecurityTokenInvalidAudienceException:
                                Log.Warning("Token has an invalid audience.");
                                break;
                        }

                        return Task.CompletedTask;
                    }
                };
            });;

        Log.Information("  Registering JWT authentication policies");
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicies.InitialAzureAdJwtAuthPolicy, policy =>
                policy.RequireAuthenticatedUser().AddAuthenticationSchemes(AuthSchemes.InitialAzureAdJwtAuthScheme))
            .AddPolicy(AuthPolicies.DefaultJwtAuthPolicy, policy =>
                policy.RequireAuthenticatedUser().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

        Log.Information("  Registering role authorization policies");
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicies.InternalUserPolicy,
                x => x.RequireAuthenticatedUser().RequireRole(AuthRoles.Internal))
            .AddPolicy(AuthPolicies.ExternalUserPolicy,
                x => x.RequireAuthenticatedUser().RequireRole(AuthRoles.External));
        
        Log.Information("  Registering custom authorization handler-requirement policies");
        services.AddTransient<IAuthorizationHandler, AdministratorHandler>();
        services.AddTransient<IAuthorizationHandler, CandidateWorkflowBelongsToRequesterHandler>();
        services.AddAuthorization(Policies.SetupPolicies);

        Log.Information("  Initializing TwilioClient");
        TwilioClient.Init(authConfig.Twilio.AccountSid, authConfig.Twilio.AuthToken);
    }
}