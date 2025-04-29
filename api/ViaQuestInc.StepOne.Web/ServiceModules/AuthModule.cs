using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Twilio;
using ViaQuestInc.StepOne.Core.Auth;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;
using ViaQuestInc.StepOne.Core.Auth.Services;
using ViaQuestInc.StepOne.Kernel.Auth;
using ViaQuestInc.StepOne.Web.Auth;
using ViaQuestInc.StepOne.Web.Auth.RequirementsAndHandlers;

namespace ViaQuestInc.StepOne.Web.ServiceModules;

public class AuthModule : IServiceModule
{
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
        services.AddAuthentication()
            .AddJwtBearer(Schemes.InitialAzureAdJwtAuthScheme, options =>
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
                    OnForbidden = v =>
                    {
                        var nameIdentifier = v.HttpContext.User.GetNameIdentifier();

                        Log.Warning(
                            $"403 FORBIDDEN: {nameIdentifier} with IP {v.HttpContext.Connection.RemoteIpAddress} - [{v.Request.Method} {v.Request.Path}]");

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = v =>
                    {
                        Log.Information("Azure AD token validation PASSED for scheme {Scheme}.", v.Scheme);

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = v =>
                    {
                        Log.Error("Azure AD token validation FAILED for scheme {Scheme}: {Message} {Stack}", v.Scheme,
                            v.Exception.Message,
                            v.Exception.StackTrace);

                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(Schemes.NativeJwtAuthScheme, options =>
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
                    OnForbidden = v =>
                    {
                        var nameIdentifier = v.HttpContext.User.GetNameIdentifier();

                        Log.Warning(
                            $"403 FORBIDDEN: {nameIdentifier} with IP {v.HttpContext.Connection.RemoteIpAddress} - [{v.Request.Method} {v.Request.Path}]");

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = v =>
                    {
                        Log.Information("Native JWT validation PASSED for scheme {Scheme}.", v.Scheme);

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = v =>
                    {
                        Log.Error("Native JWT validation FAILED for scheme {Scheme}: {Message} {Stack}", v.Scheme,
                            v.Exception.Message,
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
            });
        ;

        Log.Information("  Registering authentication policies");
        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.AzureAdJwtAuthPolicy, x =>
                x.RequireAuthenticatedUser().AddAuthenticationSchemes(Schemes.InitialAzureAdJwtAuthScheme))
            .AddPolicy(Policies.NativeJwtAuthPolicy, x =>
                x.RequireAuthenticatedUser().AddAuthenticationSchemes(Schemes.NativeJwtAuthScheme))
            .AddPolicy(Policies.InternalUserPolicy, x =>
                x.RequireAuthenticatedUser().RequireRole(Roles.Internal)
                    .AddAuthenticationSchemes(Schemes.NativeJwtAuthScheme))
            .AddPolicy(Policies.ExternalUserPolicy, x =>
                x.RequireAuthenticatedUser().RequireRole(Roles.External)
                    .AddAuthenticationSchemes(Schemes.NativeJwtAuthScheme));

        Log.Information("  Registering custom authorization handler-requirement policies");
        services.AddTransient<IAuthorizationHandler, AdministratorHandler>();
        services.AddTransient<IAuthorizationHandler, CandidateWorkflowBelongsToRequesterHandler>();
        services.AddAuthorization(Policies.SetupPolicies);

        Log.Information("  Initializing TwilioClient");
        TwilioClient.Init(authConfig.Twilio.AccountSid, authConfig.Twilio.AuthToken);
    }
}