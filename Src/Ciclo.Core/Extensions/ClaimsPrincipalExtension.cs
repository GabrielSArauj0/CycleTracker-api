using System.Security.Claims;
using Ciclo.Core.Authorization;

namespace Ciclo.Core.Extensions;

public static class ClaimsPrincipalExtension
{ 
    public static bool VerificarPermissao(this ClaimsPrincipal? user, string claimName, string claimValue)
    {
        if (user is null)
        {
            return false;
        }
        
        return user
            .Claims
            .Where(p => p.Type == "permissoes")
            .Any(p => PermissaoClaim.Verificar(p.Value, claimName, claimValue));
    }
    public static bool UsuarioAutenticado(this ClaimsPrincipal? principal)
    {
        return principal?.Identity?.IsAuthenticated ?? false;
    }

    public static string? ObterUsuarioId(this ClaimsPrincipal? principal) => GetClaim(principal, ClaimTypes.NameIdentifier);
    public static string? ObterTipoUsuario(this ClaimsPrincipal? principal) => GetClaim(principal, "TipoUsuario");
    public static string? ObterNomeUsuario(this ClaimsPrincipal? principal) => GetClaim(principal, ClaimTypes.Name);
    public static string? ObterEmailUsuario(this ClaimsPrincipal? principal) => GetClaim(principal, ClaimTypes.Email);

    private static string? GetClaim(ClaimsPrincipal? principal, string claimName)
    {
        if (principal == null)
        {
            throw new ArgumentException(null, nameof(principal));
        }

        var claim = principal.FindFirst(claimName);
        return claim?.Value;
    }
}