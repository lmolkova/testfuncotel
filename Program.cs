using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Builder;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using Microsoft.Extensions.Logging;

AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

var builder = FunctionsApplication.CreateBuilder(args);

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});


builder.Services.AddOpenTelemetry()
    .WithTracing(t => t
            .AddSource("Microsoft.Azure.Functions.Worker")
            .AddSource("Azure.*")
            .AddSource("testme*")
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter())
    .WithMetrics(m => m
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation())
    .UseFunctionsWorkerDefaults()
    .UseOtlpExporter();

var host = builder
    .Build();

host.Run();
