using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Builder;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using Microsoft.Extensions.Logging;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Resources;

//AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

//var builder = FunctionsApplication.CreateBuilder(args);
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(s => s.AddOpenTelemetry()
        .WithTracing(t => t
                .AddSource("testme")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
        .WithMetrics(m => m
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation())
    .UseFunctionsWorkerDefaults()
    .UseOtlpExporter())
    .Build();

host.Run();
