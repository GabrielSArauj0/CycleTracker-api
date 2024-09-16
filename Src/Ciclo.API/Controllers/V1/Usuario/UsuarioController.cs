using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Administrador;
using Ciclo.Application.Dtos.V1.Usuario;
using Ciclo.Application.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ciclo.API.Controllers.V1.Usuario;

[Authorize]
[Microsoft.AspNetCore.Components.Route("v{version:apiVersion}/[controller]")]
public class UsuarioController : MainController
{
    private readonly IUsuarioService _usuarioService;
    
    public UsuarioController(INotificator notificator, IUsuarioService usuarioService) : base(notificator)
    {
        _usuarioService = usuarioService;
    }
    
    [AllowAnonymous]
    [HttpPost]
    [SwaggerOperation(Summary = "Adicionar um usuario.", Tags = new [] { "Usuario - Usuario" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarUsuarioDto dto)
    {
        return CreatedResponse("", await _usuarioService.Adicionar(dto));
    }
}