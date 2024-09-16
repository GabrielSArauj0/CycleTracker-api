using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Auth;
using Ciclo.Application.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ciclo.API.Controllers.V1.Usuario;

[AllowAnonymous]
[Microsoft.AspNetCore.Components.Route("v{version:apiVersion}/[controller]")]
public class UsuarioAuthController : BaseController
{
    private readonly IUsuarioAuthService _usuarioAuthService;
    
    public UsuarioAuthController(INotificator notificator, IUsuarioAuthService usuarioAuthService) : base(notificator)
    {
        _usuarioAuthService = usuarioAuthService;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Login de Usuario.", Tags = new [] { "Usuario - Auth" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Adicionar([FromBody] LoginDto dto)
    {
        var token = await _usuarioAuthService.Login(dto);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Email e/ou senha incorretos" });
    }
}