using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Recetas;
using VetConecta.Application.Recetas.Commands.CrearReceta;
using VetConecta.Application.Recetas.Commands.EliminarReceta;
using VetConecta.Application.Recetas.Queries.GetRecetas;

namespace VetConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecetasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RecetaDto>>> GetRecetas()
        => Ok(await sender.Send(new GetRecetasQuery()));

    [HttpPost]
    public async Task<ActionResult<RecetaDto>> CrearReceta(CrearRecetaCommand command)
        => Ok(await sender.Send(command));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> EliminarReceta(Guid id)
    {
        await sender.Send(new EliminarRecetaCommand(id));
        return NoContent();
    }
}
