namespace Ciclo.Application.Dtos.V1.Usuario;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Ativo { get; set; }
}