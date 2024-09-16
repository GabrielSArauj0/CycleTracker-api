namespace Ciclo.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}