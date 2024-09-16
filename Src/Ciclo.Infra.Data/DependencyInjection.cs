using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ciclo.Core.Authorization;
using Ciclo.Core.Extensions;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Infra.Data.Context;
using Ciclo.Infra.Data.Repositories;

namespace Ciclo.Infra.Data;

public static class DependencyInjection
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IAuthenticatedUser>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

            return httpContextAccessor.UsuarioAutenticado() ? new AuthenticatedUser(httpContextAccessor) : new AuthenticatedUser();
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }

    public static void RepositoryDependency(this IServiceCollection service)
    {
        service.AddScoped<IAdministradorRepository, AdministradorRepository>();
        service.AddScoped<IUsuarioRepository, UsuarioRepository>();
        service.AddScoped<ICicloMenstrualRepository, CicloMenstrualRepository>();
    }

    public static void UseMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}