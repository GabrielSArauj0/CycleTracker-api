using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Ciclo.Core.Authorization;
using Ciclo.Domain.Contracts;
using Ciclo.Domain.Entities;
using Ciclo.Infra.Data.Extensions;

namespace Ciclo.Infra.Data.Context;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IAuthenticatedUser _authenticatedUser;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUser authenticatedUser) : base(options)
    {
        _authenticatedUser = authenticatedUser;
    }

    public DbSet<Administrador> Administradores { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<CicloMenstrual> CicloMenstruals = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyConfigurations(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ApplyTrackingChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTrackingChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ITracking && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((ITracking)entityEntry.Entity).AtualizadoEm = DateTime.Now;
            ((ITracking)entityEntry.Entity).AtualizadoPor = _authenticatedUser.ObterIdentificador();
            
            if (entityEntry.State != EntityState.Added)
                continue;

            ((ITracking)entityEntry.Entity).CriadoEm = DateTime.Now;
            ((ITracking)entityEntry.Entity).CriadoPor = _authenticatedUser.ObterIdentificador();
        }
    }

    private static void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        
        modelBuilder.ApplyEntityConfiguration();
        modelBuilder.ApplyTrackingConfiguration();
        modelBuilder.ApplySoftDeleteConfiguration();
    }
}