using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Localization;
using Ciclo.API.Configurations;
using Ciclo.Core.OpenTelemetry;
using Ciclo.Application;
using Ciclo.Application.BackgroundJob;
using Ciclo.Application.BackgroundJob.Job;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Configure<RequestLocalizationOptions>(o =>
    {
        var supportedCultures = new[] { new CultureInfo("pt-BR") };
        o.DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR");
        o.SupportedCultures = supportedCultures;
        o.SupportedUICultures = supportedCultures;
    });

builder
    .Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true, true)
    .AddEnvironmentVariables();

builder
    .Services
    .SetupSettings(builder.Configuration);

builder
    .Services
    .ConfigureOpenTelemetry(builder.Configuration, builder.Environment);

builder
    .Services
    .AddResponseCompression(options => { options.EnableForHttps = true; });

builder
    .Services
    .AddApiConfiguration();

builder.Services.ConfigureApplication(builder.Configuration, builder.Environment);
builder.Services.AddServices();

builder
    .Services.AddVersioning();

builder
    .Services
    .AddSwagger();

builder
    .Services
    .AddHealthChecks()
    .ConfigureApplicationHealthChecks(builder.Configuration);

builder
    .Services
    .AddAuthenticationConfig(builder.Configuration, builder.Environment);

builder
    .Services
    .AddSingleton<IWorkerParams, WorkerParams>()
    .AddScoped<AtualizarAvisosJob>();

var app = builder.Build();

app.UseApiConfiguration(app.Services, app.Environment);

app.UseSwaggerConfig();

app.UseHttpsRedirection();

app.UseAuthenticationConfig();

app.UseStaticFileConfiguration(app.Configuration);

app.UseApplicationHealthCheck();

app.MapControllers();

await app.RunAsync();