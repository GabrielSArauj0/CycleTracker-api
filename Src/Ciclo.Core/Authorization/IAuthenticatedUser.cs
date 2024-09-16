using Ciclo.Core.Enums;
using Microsoft.AspNetCore.Http;
using Ciclo.Core.Extensions;

namespace Ciclo.Core.Authorization;

public interface IAuthenticatedUser
{
    public int Id { get; }
    public ETipoUsuario? TipoUsuario { get; }    
    public string Nome { get; }
    public string Email { get; }
    
    
    public bool UsuarioLogado { get; }
    public bool UsuarioComum { get; }
    public bool UsuarioAdministrador { get; }

    int ObterIdentificador();
}

public class AuthenticatedUser : IAuthenticatedUser
{
    public int Id { get; } = -1;
    public ETipoUsuario? TipoUsuario { get; }    
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool UsuarioLogado => Id > 0;
    public bool UsuarioComum => TipoUsuario is ETipoUsuario.Comum;
    public bool UsuarioAdministrador => TipoUsuario is ETipoUsuario.Administrador;
    
    public AuthenticatedUser()
    { }
    
    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
    {
        Id = httpContextAccessor.ObterUsuarioId()!.Value;
        TipoUsuario = httpContextAccessor.ObterTipoUsuario()!.Value;

        Nome = httpContextAccessor.ObterNome();
        Email = httpContextAccessor.ObterEmail();
    }

    public int ObterIdentificador() => Id;
}