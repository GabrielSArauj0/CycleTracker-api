using Ciclo.Application.Dtos.V1.CicloMenstrual;
using Ciclo.Application.Dtos.V1.CicloMenstrual.FaseCiclo;

namespace Ciclo.Application.Contracts;

public interface ICicloMenstrualService
{
    Task<CicloMenstrualDto?> Adicionar(AdicionarCicloMenstrualDto dto);
    Task<CicloMenstrualDto?> Atualizar(int id, AtualizarCicloMenstrualDto dto);
    Task<CicloMenstrualDto?> ObterPorId(int id);
    Task<List<FaseCicloDto?>> CalculoCiclo(int cicloId);
}