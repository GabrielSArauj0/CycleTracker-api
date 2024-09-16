using Ciclo.Application.Dtos.V1.Auth;

namespace Ciclo.Application.Contracts;

public interface IUsuarioAuthService
{
    Task<TokenDto?> Login(LoginDto login);
}