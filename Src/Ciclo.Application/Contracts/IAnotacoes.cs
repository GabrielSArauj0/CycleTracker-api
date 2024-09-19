using Ciclo.Application.Dtos.V1.Anotacoes;

namespace Ciclo.Application.Contracts;

public interface IAnotacoes
{
    Task<AnotacaoDto?> Adicionar(AdicionarAnotacaoDto dto);
    Task<AnotacaoDto?> Atualizar(int id, AtualizarAnotacaoDto dto);
    Task<AnotacaoDto?> ObterPorId(int id);
    Task<List<AnotacaoDto?>> CalculoCiclo(int cicloId);
}