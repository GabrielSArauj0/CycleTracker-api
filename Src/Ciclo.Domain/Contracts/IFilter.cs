namespace Ciclo.Domain.Contracts;

public interface IFilter<T> where T : IEntity
{
    void ApplyFilter(ref IQueryable<T> queryable);
}