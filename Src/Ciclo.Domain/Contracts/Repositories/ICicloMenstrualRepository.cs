using Ciclo.Domain.Entities;

namespace Ciclo.Domain.Contracts.Repositories;

public interface ICicloMenstrualRepository : IRepository<CicloMenstrual>
{
    void Cadastrar(CicloMenstrual cicloMenstrual);
    void Atualizar(CicloMenstrual cicloMenstrual);
    Task<CicloMenstrual?> ObterPorId(int id);
}