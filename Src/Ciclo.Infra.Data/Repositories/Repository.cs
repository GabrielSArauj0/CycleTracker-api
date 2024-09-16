using System.Linq.Expressions;
using Ciclo.Domain.Entities;
using Ciclo.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Ciclo.Domain.Contracts.Repositories;
using Ciclo.Infra.Data.Context;

namespace Ciclo.Infra.Data.Repositories;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot
{
    protected readonly ApplicationDbContext Context;
    private readonly DbSet<T> _dbSet;
    private bool _isDisposed;

    protected Repository(ApplicationDbContext context)
    {
        Context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> Buscar(IFilter<T> filtro, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();
        filtro.ApplyFilter(ref query);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<T>> Buscar(Expression<Func<T, bool>> expression, bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(expression).AsQueryable();

        return asNoTracking
            ? await query.AsNoTracking().ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(expression);
    }

    public async Task<bool> Any(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }
    
    public IUnitOfWork UnitOfWork => Context;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            Context.Dispose();
        }

        _isDisposed = true;
    }
    
    ~Repository()
    {
        Dispose(false);
    }
}