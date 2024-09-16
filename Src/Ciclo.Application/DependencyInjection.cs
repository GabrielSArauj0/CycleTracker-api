using System.Reflection;
using System.Text.Json.Serialization;
using Ciclo.Application.Contracts;
using Ciclo.Application.Notifications;
using Ciclo.Application.Services;
using Ciclo.Core.Enums;
using Ciclo.Core.Extensions;
using Ciclo.Core.Settings;
using Ciclo.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;
using Ciclo.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Ciclo.Application;

public static class DependencyInjection
{
    public static void SetupSettings(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        service.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        service.Configure<UploadSettings>(configuration.GetSection("UploadSettings"));
    }

    public static void ConfigureApplication(this IServiceCollection service, IConfiguration configuration, IWebHostEnvironment environment)
    {
        service
            .ConfigureDbContext(configuration);
        
        service
            .RepositoryDependency();
        
        service
            .AddSignalR(options => { options.EnableDetailedErrors = !environment.IsProduction(); })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.MaxDepth = 5;
                options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        
        service
            .AddAutoMapper(Assembly.GetExecutingAssembly());
    }
    
    public static void AddServices(this IServiceCollection service)
    {
        service.AddScoped<IPasswordHasher<Administrador>, Argon2PasswordHasher<Administrador>>();

        service.AddScoped<IPasswordHasher<Usuario>, Argon2PasswordHasher<Usuario>>();
        
        service.AddScoped<INotificator, Notificator>();

        service.AddScoped<IFileService, FileService>();

        service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        service
            .AddScoped<IAdministradorAuthService, AdministradorAuthService>()
            .AddScoped<IUsuarioAuthService, UsuarioAuthService>()
            .AddScoped<IAdministradorService, AdministradorService>()
            .AddScoped<IUsuarioService, UsuarioService>()
            .AddScoped<ICicloMenstrualService, CicloMenstrualService>();
    }
    
    public static void UseStaticFileConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        var uploadSettings = configuration.GetSection("UploadSettings");
        var publicBasePath = uploadSettings.GetValue<string>("PublicBasePath");
        
        var absolutePublicBasePath = Path.GetFullPath(publicBasePath!);

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(absolutePublicBasePath),
            RequestPath = $"/{EPathAccess.Public.ToDescriptionString()}"
        });
        
    }
}