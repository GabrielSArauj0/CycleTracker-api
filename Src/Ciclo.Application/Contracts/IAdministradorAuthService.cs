using Ciclo.Application.Dtos.V1.Auth;

namespace Ciclo.Application.Contracts;

public interface IAdministradorAuthService
{
    Task<TokenDto?> Login(LoginDto login);
}