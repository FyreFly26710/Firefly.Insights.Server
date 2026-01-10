using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

namespace Server.Gateway.Api;

public static class ProgramExtensions
{
    public static WebApplicationBuilder AddDefaultLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var projectName = Assembly.GetEntryAssembly()?.GetName().Name ?? "UnknownService";
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithProperty("ApplicationName", projectName)
            .Enrich.WithProperty("ThreadId", Environment.CurrentManagedThreadId);

        loggerConfiguration.WriteTo.Seq("http://localhost:5341");

        loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            loggerConfiguration.MinimumLevel.Override("Yarp", LogEventLevel.Debug);
            
            const string customTemplate = "{Timestamp: HH:mm:ss} [{Level:u3}] {SourceContext} {NewLine} {Message:lj}{NewLine}{Exception}";
            loggerConfiguration.WriteTo.Console(
                theme: AnsiConsoleTheme.Literate,
                outputTemplate: customTemplate);
        }
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
        {
            string connString = configuration.GetValue<string>("ApplicationInsights:ConnectionString") ?? string.Empty;

            loggerConfiguration.WriteTo.ApplicationInsights(
                connectionString: connString,
                telemetryConverter: TelemetryConverter.Traces);
        }
        Log.Logger = loggerConfiguration.CreateLogger();
        builder.Services.AddSerilog();

        return builder;
    }
}
