using Ciclo.Domain.Contracts;

namespace Ciclo.Application.Dtos.Base;

public abstract class BaseFilterDto<T> : IFilter<T> where T : IEntity
{
    public virtual void ApplyFilter(ref IQueryable<T> queryable) { }
}