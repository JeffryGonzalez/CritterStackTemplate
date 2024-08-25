using Marten;
using Marten.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Wolverine;
using Wolverine.Marten;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oakton;
using Oakton.Resources;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace CritterStack;

public static class JasperFxExtensions
{
    private static string _serviceName = null!;
    public static WebApplicationBuilder UseJasperFxStackWithReasonableDefaults(this WebApplicationBuilder builder, string connectionStringKey = "CritterStack")
    {
        builder.Host.ApplyOaktonExtensions();
 
        
        var connectionString = builder.Configuration.GetConnectionString(connectionStringKey) ??
                               throw new Exception("Missing connection string");
        builder.Services.AddMarten(config =>
        {
            config.OpenTelemetry.TrackConnections = TrackLevel.Verbose;
            config.OpenTelemetry.TrackEventCounters();
            config.Connection(connectionString);
        }).UseLightweightSessions().IntegrateWithWolverine();
        builder.Host.UseWolverine(opts =>
        {
            
            opts.Policies.LogMessageStarting(LogLevel.Debug);
            opts.Policies.UseDurableInboxOnAllListeners();
            opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
            opts.Policies.AutoApplyTransactions();

            _serviceName = opts.ServiceName;
        }).UseResourceSetupOnStartupInDevelopment();
        builder.Services.ConfigureHttpClientDefaults(http => { http.AddStandardResilienceHandler(); });
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeScopes = true;
            logging.IncludeFormattedMessage = true;
        });
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddSource("Marten")
                    .AddSource("Wolverine");
            }).WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation().AddRuntimeInstrumentation()
                    .AddMeter("EventPublisher").AddMeter("Marten").AddMeter($"Wolverine:{_serviceName}");

            });
        builder.AddOpenTelemetryExporters();
        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions { Predicate = r => r.Tags.Contains("live") });
        }

        return app;
    }
}