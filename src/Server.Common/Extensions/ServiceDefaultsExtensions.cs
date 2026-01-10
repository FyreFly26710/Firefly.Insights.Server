using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Server.Common.Configurations;
using Server.Common.Utils;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Server.Common.Extensions
{
    public static class ServiceDefaultsExtensions
    {

        public static IServiceCollection AddDefaultServices(this IServiceCollection services, IConfiguration configuration)
        {
            var machineId = configuration.GetValue<long>("Snowflake:MachineId");
            SnowflakeId.Initialize(machineId);

            services.AddJwt(configuration);

            return services;
        }

        public static WebApplicationBuilder AddDefaultLogging(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var projectName = Assembly.GetEntryAssembly()?.GetName().Name ?? "UnknownService";

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("ApplicationName", projectName)
                .Enrich.WithProperty("ThreadId", Environment.CurrentManagedThreadId);

            // Global Overrides
            loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);
                
            var seqUrl = configuration.GetValue<string>("Serilog:SeqUrl") ?? "http://localhost:5341";
            loggerConfiguration.WriteTo.Seq(seqUrl);

            if (EnvUtil.IsDevelopment())
            {
                const string customTemplate = "{Timestamp: HH:mm:ss} [{Level:u3}] {SourceContext} {NewLine} {Message:lj}{NewLine}{Exception}";
                loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Literate, outputTemplate: customTemplate);
            }

            if (EnvUtil.IsProduction())
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
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {


            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // optional: remove default 5 min tolerance
                };
            });


            return services;
        }
    }
}
