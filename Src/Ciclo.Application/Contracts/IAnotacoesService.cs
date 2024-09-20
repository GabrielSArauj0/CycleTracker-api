using Ciclo.Application.Dtos.V1.Anotacoes;

namespace Ciclo.Application.Contracts;

public interface IAnotacoesService
{
    Task<AnotacaoDto?> Adicionar(AtualizarAnotacaoDto dto);
    Task<AnotacaoDto?> Atualizar(int id, AtualizarAnotacaoDto dto);
    Task<AnotacaoDto?> ObterPorId(int id);
    
    Task<List<AnotacaoDto>> AnotacoesUser(int userId);
    
}