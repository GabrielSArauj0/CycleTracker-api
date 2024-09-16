using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Auth;
using Ciclo.Application.Notifications;
using Swashbuckle.AspNetCore.Annotations;

namespace Ciclo.API.Controllers.V1.Administracao;

[AllowAnonymous]
[Route("v{version:apiVersion}/[controller]")]
public class AdministradoresAuthController : BaseController
{
    private readonly IAdministradorAuthService _administradorAuthService;
    public AdministradoresAuthController(INotificator notificator, IAdministradorAuthService administradorAuthService) : base(notificator)
    {
        _administradorAuthService = administradorAuthService;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Login de administrador.", Tags = new [] { "Administração - Auth" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Adicionar([FromBody] LoginDto dto)
    {
        var token = await _administradorAuthService.Login(dto);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Email e/ou senha incorretos" });
    }
}