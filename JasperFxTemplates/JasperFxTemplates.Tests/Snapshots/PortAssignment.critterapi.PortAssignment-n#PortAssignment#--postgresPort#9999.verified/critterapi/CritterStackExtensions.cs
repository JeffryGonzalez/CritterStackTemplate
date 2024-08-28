using JasperFx.CodeGeneration;
using Marten;
using Marten.Services;
using Oakton;
using Oakton.Resources;
using Weasel.Core;
using Wolverine;
using Wolverine.Marten;

namespace PortAssignment;

public static class CritterStackExtensions
{
    private static string _serviceName = "";
    public static WebApplicationBuilder AddCritterStackWebHost(this WebApplicationBuilder builder, string connectionStringKey)
    {
        builder.Host.ApplyOaktonExtensions();
        var connectionString = builder.Configuration.GetConnectionString(connectionStringKey) ??
                               throw new Exception("Missing connection string");
        builder.Services.AddMarten(config =>
        {
            config.OpenTelemetry.TrackConnections = TrackLevel.Verbose;
            config.OpenTelemetry.TrackEventCounters();
            config.Connection(connectionString);
            config.AutoCreateSchemaObjects = builder.Environment.IsDevelopment() ?
                AutoCreate.CreateOrUpdate // The Default
                : AutoCreate.None; // see https://martendb.io/schema/migrations.html#development-time-with-auto-create-mode
        }).UseLightweightSessions().IntegrateWithWolverine();
        builder.Host.UseWolverine(opts =>
        {
            opts.Policies.LogMessageStarting(LogLevel.Debug);
            opts.Policies.UseDurableInboxOnAllListeners();
            opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
            opts.Policies.AutoApplyTransactions();
            opts.CodeGeneration.TypeLoadMode = builder.Environment.IsDevelopment() ?
                TypeLoadMode.Dynamic // Can change to TypeLoadMode.Auto if devex gets slow, see https://wolverinefx.io/guide/codegen.html#working-with-code-generation
                : TypeLoadMode.Static;
            _serviceName = opts.ServiceName;
        }).UseResourceSetupOnStartupInDevelopment();
        builder.ConfigureOpenTelemetry(); 
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
    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        return builder;
    }
}
