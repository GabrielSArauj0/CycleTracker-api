using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using B3Propagator = OpenTelemetry.Extensions.Propagators.B3Propagator;

namespace Ciclo.Core.OpenTelemetry;

public static class Extensions
{
    public static void ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var openTelemetry = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConfig>();
        if (openTelemetry is null || !openTelemetry.Ativo)
        {
            return;
        }
        
        Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(new TextMapPropagator[]
        {
            new B3Propagator(), new BaggagePropagator()
        }));
        
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource
                    .AddService(serviceName: openTelemetry.ServiceName)
                    .AddContainerDetector()
                    .AddHostDetector()
                    .AddProcessDetector()
                    .AddProcessRuntimeDetector()
                    .AddEnvironmentVariableDetector()
                    .AddAttributes(new List<KeyValuePair<string, object>>
                    {
                        new("deployment.environment", environment.EnvironmentName)
                    });
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter = context =>
                        {
                            if (context.Request.Method.Equals("options", StringComparison.CurrentCultureIgnoreCase))
                            {
                                return false;
                            }
                            
                            return !context.Request.Path.StartsWithSegments("/swagger", StringComparison.CurrentCultureIgnoreCase);
                        };
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = EnrichWithHttpRequest;
                        options.EnrichWithHttpResponse = EnrichWithHttpResponse;
                        options.EnrichWithException = EnrichWithException;
                    })
                    .AddEntityFrameworkCoreInstrumentation(o =>
                    {
                        o.EnrichWithIDbCommand = (activity, command) => activity.ReplaceDbParamValues(command);
                        o.SetDbStatementForText = true;
                        o.SetDbStatementForStoredProcedure = true;
                    })
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(openTelemetry.OtlpEndpoint);
                        otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                    });
            })
            .WithLogging(logging =>
            {
                logging
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(openTelemetry.OtlpEndpoint);
                        otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(openTelemetry.OtlpEndpoint);
                        otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                    });
            });
    }
    
    private static void EnrichWithHttpRequest(Activity activity, HttpRequest httpRequest)
    {
        activity.SetTag("request.protocol", httpRequest.Protocol);

        activity.AddRequestBody(httpRequest);
        
        activity.AddAuthInfos(httpRequest);
    }

    private static void EnrichWithHttpResponse(Activity activity, HttpResponse httpResponse)
    {
        activity.SetTag("Response.Length", httpResponse.ContentLength);
        activity.SetTag("Response.StatusCode", httpResponse.StatusCode);
        activity.SetTag("Response.ContentType", httpResponse.ContentType);
    }

    private static void EnrichWithException(Activity activity, Exception exception)
    {
        activity.SetTag("Exception.Type", exception.GetType().ToString());
    }
}