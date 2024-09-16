using Ciclo.Domain.Entities;

namespace Ciclo.Domain.Contracts.Repositories;

public interface IAdministradorRepository : IRepository<Administrador>
{
    void Cadastrar(Administrador administrador);
    void Atualizar(Administrador administrador);
    Task<Administrador?> ObterPorId(int id);
    Task<Administrador?> ObterPorEmail(string email);
    Task<List<Administrador>> ObterTodos();
}