using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Serilog;
using ViaQuestInc.StepOne.Kernel;
using ViaQuestInc.StepOne.Web;
using ViaQuestInc.StepOne.Web.Configuration;
using ViaQuestInc.StepOne.Web.ServiceModules;
using ViaQuestInc.StepOne.Web.ServicesManagement;
using ViaQuestInc.StepOne.Web.StartupActions;

const string migrationsModeSwitch = "--migrations-mode";
const string envVarsPrefix = "STEPONE_";
const string aspnetEnvironmentEnvVar = "ASPNETCORE_ENVIRONMENT";

try
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    Cultures.SetSystemCulture("en-US");

    var environmentName = Environment.GetEnvironmentVariable(aspnetEnvironmentEnvVar);

    var builder = WebApplication.CreateBuilder(
        new WebApplicationOptions
        {
            EnvironmentName = environmentName,
            ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
        }
    );

    builder.Host.UseSerilog();

    var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false);

    if (!string.IsNullOrEmpty(environmentName))
    {
        configurationBuilder.AddJsonFile($"appsettings.{environmentName}.json", true);
    }

    configurationBuilder.AddEnvironmentVariables(envVarsPrefix);
    configurationBuilder.AddCommandLine(args);

    var configuration = configurationBuilder.Build();

    var excludedExceptions =
        configuration.GetSection("ExcludeExceptionsFromLogging").Get<string[]>() ?? Array.Empty<string>();

    var excludedExceptionMessages = configuration.GetSection("ExcludeExceptionMessagesFromLogging").Get<string[]>() ??
                                    Array.Empty<string>();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Filter.ByExcluding(
            x => x.Exception != null &&
                 (excludedExceptions.Contains(x.Exception.GetType().FullName) ||
                  excludedExceptionMessages.Any(m => x.Exception.Message.Contains(m)))
        )
        .Enrich.FromLogContext()
        .CreateLogger();

    var banner = await Assembly.GetExecutingAssembly()
        .GetManifestResourceString("ViaQuestInc.StepOne.Web.banner.txt", Encoding.UTF8);

    Log.Warning(
        "Starting VQI StepOne web host {MigrationsMode}\n\n{Banner}\n\n",
        args.Contains(migrationsModeSwitch)
            ? "(Migrations Mode)"
            : string.Empty,
        banner
    );
    
    if (args.Contains(migrationsModeSwitch))
    {
        var serviceModulesBuilder = new ServiceModulesBuilder(builder.Services, builder.Environment, configuration);
        
        serviceModulesBuilder.AddModule<DatabaseModule>("Database");

        try
        {
            // Even though this is the method that ultimately throws the StopTheHostException and the resulting object
            // is unused, it must be invoked. See comment below.
            builder.Build();
        }
        catch (Exception ex)
        {
            // Known issue with .NET/EF Core 6:
            //     https://githubmemory.com/repo/dotnet/runtime/issues/60600
            //     https://stackoverflow.com/questions/70247187/microsoft-extensions-hosting-hostfactoryresolverhostinglistenerstopthehostexce
            // We're using a slightly different workaround in order to remove exception information from the log output
            // when performing EF migrations.
            var type = ex.GetType().Name;

            if (!type.Equals("StopTheHostException", StringComparison.Ordinal) &&
                !type.Equals("HostAbortedException", StringComparison.Ordinal))
            {
                throw;
            }

            return 0;
        }
    }

    Log.Information(
        "TIP: Use {Mode} for dotnet ef commands, e.g., {Cmd}",
        "--migrations-mode",
        "dotnet ef dbcontext info -- --migrations-mode"
    );
    
    const string stepOneCorsPolicy = nameof(stepOneCorsPolicy);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(stepOneCorsPolicy,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200") // TODO
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });
    
    builder.Services.AddHttpContextAccessor()
        .AddHttpClient()
        .AddRouting(options => options.LowercaseUrls = true)
        .AddEndpointsApiExplorer()
        .AddStepOneConfigurations(builder.Environment, configuration)
        .AddStepOneModules(builder.Environment, configuration)
        .AddStepOneServices(configuration)
        .AddControllers();
    
    var app = builder.Build();

    if (string.IsNullOrEmpty(app.Environment.EnvironmentName))
    {
        Log.Fatal("Environment variable {EnvVar} not set.", aspnetEnvironmentEnvVar);
        Log.Fatal("  See: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-9.0");
        Log.Fatal(
            "  Windows: In PowerShell w/admin rights, run {Cmd}",
            "[Environment]::SetEnvironmentVariable(\"ASPNETCORE_ENVIRONMENT\", \"Local|Development|Staging|Production\", \"Machine\")"
        );
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var enableSsl = configuration.GetValue<bool>("EnableSsl");

    Log.Information("SSL Enabled: {EnableSsl}", enableSsl);

    if (!app.Environment.IsLocal())
    {
        if (!enableSsl)
        {
            throw new("SSL must be enabled in all deployed environments. Set EnableSsl to true in appsettings.json.");
        }

        app.UseExceptionHandler("/Error");
    }

    if (enableSsl)
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsLocalOrDevelopment())
    {
        app.UseDeveloperExceptionPage();
        IdentityModelEventSource.ShowPII = true;
    }

    // https://github.com/serilog/serilog-aspnetcore
    app.UseSerilogRequestLogging();

    var serverConfig = app.Services.GetService<IOptions<ServerConfig>>();
    
    if (!string.IsNullOrEmpty(serverConfig?.Value.PathBase))
    {
        Log.Information("Server Path Base: {PathBase}", serverConfig.Value.PathBase);
        app.UsePathBase(serverConfig.Value.PathBase);
    }
    
    app.UseRouting();
    app.UseCors(stepOneCorsPolicy);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers().RequireAuthorization();
    
    // Startup Actions
    using (var scope = app.Services.CreateScope())
    {
        foreach (var startupAction in scope.ServiceProvider.GetRequiredService<IEnumerable<IStartupAction>>())
        {
            if (!await startupAction.ShouldExecuteAsync(app)) continue;

            Log.Information("");
            Log.Information("Executing Start-Up Action {StartUpAction}", startupAction.GetType().Name);

            await startupAction.OnStartupAsync(app);
        }
    }
    
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var server = app.Services.GetRequiredService<IServer>();
        var addressFeature = server.Features.Get<IServerAddressesFeature>();

        if (addressFeature?.Addresses is not null)
        {
            foreach (var address in addressFeature.Addresses)
            {
                Log.Information("Application is listening on {Address}.", address);
            }
        }
    });

    stopwatch.Stop();
    
    Log.Information("");
    Log.Information("-------------------------------------------------");
    Log.Information(
        "{AssemblyName} started in {EnvironmentName} in {ElapsedSeconds}s",
        app.Environment.ApplicationName,
        app.Environment.EnvironmentName,
        stopwatch.Elapsed.TotalSeconds.ToString("N1")
    );
    Log.Information("------------------ {ready} ------------------", "<< READY >>");

    await app.RunAsync();

    return 0;
}
catch (Exception e)
{
    Console.Error.WriteLine("APPLICATION TERMINATED UNEXPECTEDLY");
    Console.Error.WriteLine(e);

    await Log.CloseAndFlushAsync();

    return 1;
}