using Ciclo.Domain.Entities;

namespace Ciclo.Domain.Contracts.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    void Cadastrar(Usuario usuario);
    void Atualizar(Usuario usario);
    Task<Usuario?> ObterPorId(int id);
    Task<Usuario?> ObterPorEmail(string email);
    Task<List<Usuario>> ObterTodos();
}