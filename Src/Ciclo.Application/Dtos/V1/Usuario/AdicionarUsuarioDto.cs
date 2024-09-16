namespace Ciclo.Application.Dtos.V1.Usuario;

public class AdicionarUsuarioDto
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}