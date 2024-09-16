using Ciclo.Application.Dtos.V1.Usuario;

namespace Ciclo.Application.Contracts;

public interface IUsuarioService
{
    Task<UsuarioDto?> Adicionar(AdicionarUsuarioDto dto);
    Task<UsuarioDto?> Atualizar(int id, UsuarioDto dto);
    Task<UsuarioDto?> ObterPorId(int id);
    Task<UsuarioDto?> ObterPorEmail(string email);
    Task<List<UsuarioDto>> ObterTodos();
    Task Ativar(int id);
    Task Desativar(int id);
}