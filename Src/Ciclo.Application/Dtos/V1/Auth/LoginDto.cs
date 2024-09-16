namespace Ciclo.Application.Dtos.V1.Auth;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}