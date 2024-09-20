using Ciclo.Domain.Entities;

namespace Ciclo.Domain.Contracts.Repositories;

public interface IAnotacoesRepository : IRepository<Anotacoes>
{
    Task<Anotacoes?> ObterPorId(int id);
    void Save(Anotacoes anotacoes);
    void Update(Anotacoes anotacoes);
    void Delete(Anotacoes anotacoes);
}