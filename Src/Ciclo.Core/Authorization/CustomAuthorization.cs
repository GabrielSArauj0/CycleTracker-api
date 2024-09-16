using System.Security.Claims;
using Ciclo.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ciclo.Core.Extensions;

namespace Ciclo.Core.Authorization;

public static class CustomAuthorization
{
    public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
    {
        return context.User.Identity!.IsAuthenticated && context.User.VerificarPermissao(claimName, claimValue);
    }

    public static bool ValidateUserType(HttpContext context, string claimValue)
    {
        return context.User.Identity!.IsAuthenticated &&
               context.User.ObterTipoUsuario() == claimValue;
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, EPermissaoTipo value) : base(typeof(RequirementClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, value.ToDescriptionString()) };
    }

    public ClaimsAuthorizeAttribute(string claimName, ETipoUsuario claimValue) : base(typeof(RequirementClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue.ToDescriptionString()) };
    }
}

public class RequirementClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public RequirementClaimFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (_claim.Type == "TipoUsuario")
        {
            if (!CustomAuthorization.ValidateUserType(context.HttpContext, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
            
            return;
        }
        
        if (!context.HttpContext.User.Identity!.IsAuthenticated)
        {
            context.Result = new StatusCodeResult(401);
            return;
        }
        
        if (!CustomAuthorization.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
        {
            context.Result = new StatusCodeResult(403);
        }
    }
}