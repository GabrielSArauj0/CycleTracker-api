namespace Ciclo.Application.Dtos.V1.Usuario;

public class AtualizarUsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string Email { get; set; } = null!;
}