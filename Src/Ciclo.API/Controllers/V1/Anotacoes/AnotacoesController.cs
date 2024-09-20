using Ciclo.Application.Contracts;
using Ciclo.Application.Dtos.V1.Anotacoes;
using Ciclo.Application.Dtos.V1.CicloMenstrual;
using Ciclo.Application.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ciclo.API.Controllers.V1.Anotacoes;

[AllowAnonymous]
[Microsoft.AspNetCore.Components.Route("v{version:apiVersion}/[controller]")]
public class AnotacoesController : MainController
{
    private readonly IAnotacoesService _anotacoesService;


    public AnotacoesController(INotificator notificator, IAnotacoesService anotacoesService) : base(notificator)
    {
        _anotacoesService = anotacoesService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Adicionar uma anotação.", Tags = new [] { "Usuario - Anotacao" })]
    [ProducesResponseType(typeof(AnotacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Adicionar([FromForm] AtualizarAnotacaoDto dto)
    {
        var result = await _anotacoesService.Adicionar(dto);
        if (result == null)
        {
            return BadRequest("Erro ao adicionar a anotação.");
        }
    
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualize uma anotação.", Tags = new [] { "Usuario - Anotacao" })]
    [ProducesResponseType(typeof(AnotacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarAnotacaoDto dto)
    {
        var result = await _anotacoesService.Atualizar(id, dto);
        if (result == null)
        {
            return NotFound();
        }
    
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter uma anotação por ID.", Tags = new [] { "Usuario - Anotacao" })]
    [ProducesResponseType(typeof(AnotacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var anotacao = await _anotacoesService.ObterPorId(id);
        if (anotacao == null)
        {
            return NotFound();
        }
        return OkResponse(anotacao);
    }
    /*
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remover uma anotação por ID.", Tags = new [] { "Usuario - Anotacao" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _anotacoesService.Delete(id);
        if (!resultado)
        {
            return NotFound(); 
        }
        return NoContent();
    }*/
    
}