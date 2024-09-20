using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Anotacoes;
using Ciclo.Application.Dtos.V1.CicloMenstrual;
using Ciclo.Application.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ciclo.API.Controllers.V1.Anotacoes;

public class AnotacoesController
{
    [AllowAnonymous]
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
    [ProducesResponseType(typeof(AnotacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Adicionar([FromForm] AdicionarCicloMenstrualDto dto)
    {
        return OkResponse(await _cicloMenstrualService.Adicionar(dto));
    }
    
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar uma anotação.", Tags = new [] { "Usuario - Anotação" })]
    [ProducesResponseType(typeof(AnotacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarCicloMenstrualDto dto)
    {
        return OkResponse(await _cicloMenstrualService.Atualizar(id,dto));
    }
    
    [HttpGet("id/{id:int}")]
    [SwaggerOperation(Summary = "Obter uma Anotação por id.", Tags = new [] { "Usuario - Anotação" })]
    [ProducesResponseType(typeof(AnotacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        return OkResponse(await _cicloMenstrualService.ObterPorId(id));
    }
    }
}