using Ciclo.Application.Dtos.V1.Administrador;

namespace Ciclo.Application.Contracts;

public interface IAdministradorService
{
    Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto);
    Task<AdministradorDto?> Atualizar(int id, AtualizarAdministradorDto dto);
    Task<AdministradorDto?> ObterPorId(int id);
    Task<AdministradorDto?> ObterPorEmail(string email);
    Task<List<AdministradorDto>> ObterTodos();
    Task Ativar(int id);
    Task Desativar(int id);
}