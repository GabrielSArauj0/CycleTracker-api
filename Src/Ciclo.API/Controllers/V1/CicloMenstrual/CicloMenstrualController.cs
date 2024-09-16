using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Administrador;
using Ciclo.Application.Dtos.V1.CicloMenstrual;
using Ciclo.Application.Dtos.V1.CicloMenstrual.FaseCiclo;
using Ciclo.Application.Notifications;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ciclo.API.Controllers.V1.CicloMenstrual;

[Microsoft.AspNetCore.Components.Route("v{version:apiVersion}/[controller]")]
public class CicloMenstrualController : MainController
{
    private readonly ICicloMenstrualService _cicloMenstrualService;
    
    public CicloMenstrualController(INotificator notificator, ICicloMenstrualService cicloMenstrualService) : base(notificator)
    {
        _cicloMenstrualService = cicloMenstrualService;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Adicionar um Cilo.", Tags = new [] { "Usuario - Ciclo" })]
    [ProducesResponseType(typeof(CicloMenstrualDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Adicionar([FromForm] AdicionarCicloMenstrualDto dto)
    {
        return OkResponse(await _cicloMenstrualService.Adicionar(dto));
    }
    
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar um Ciclo.", Tags = new [] { "Usuario - Ciclo" })]
    [ProducesResponseType(typeof(CicloMenstrualDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarCicloMenstrualDto dto)
    {
        return OkResponse(await _cicloMenstrualService.Atualizar(id,dto));
    }
    
    [HttpGet("id/{id:int}")]
    [SwaggerOperation(Summary = "Obter um Ciclo por id.", Tags = new [] { "Usuario - Ciclo" })]
    [ProducesResponseType(typeof(CicloMenstrualDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        return OkResponse(await _cicloMenstrualService.ObterPorId(id));
    }
    
    /*[HttpGet("fases/{id:int}")]
    [SwaggerOperation(Summary = "Calcular as fases de um ciclo por id.", Tags = new[] { "Usuario - Ciclo" })]
    [ProducesResponseType(typeof(List<FaseCicloDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CalcularFasesDoCiclo(int cicloId)
    {
        return OkResponse(await _cicloMenstrualService.CalculoCiclo(cicloId));
    }*/
}