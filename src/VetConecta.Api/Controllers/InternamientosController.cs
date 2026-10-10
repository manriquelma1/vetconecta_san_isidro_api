using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Internamientos;
using VetConecta.Application.Internamientos.Commands.ActualizarSignos;
using VetConecta.Application.Internamientos.Commands.AgregarEvolucion;
using VetConecta.Application.Internamientos.Commands.CambiarEstadoTratamiento;
using VetConecta.Application.Internamientos.Commands.DarAlta;
using VetConecta.Application.Internamientos.Queries.GetInternamientos;

namespace VetConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InternamientosController(ISender sender) : ControllerBase
{
    /// <summary>Personal ve todos; propietario, solo los de sus mascotas (HU-05).</summary>
    [HttpGet]
    public async Task<ActionResult<List<InternamientoDto>>> GetInternamientos()
        => Ok(await sender.Send(new GetInternamientosQuery()));

    [HttpPut("{id:guid}/signos")]
    public async Task<ActionResult<InternamientoDto>> ActualizarSignos(Guid id, ActualizarSignosCommand command)
    {
        if (id != command.Id) return BadRequest("El id de la ruta no coincide con el del cuerpo.");
        return Ok(await sender.Send(command));
    }

    [HttpPut("{internamientoId:guid}/tratamientos/{tratamientoId:guid}")]
    public async Task<ActionResult<InternamientoDto>> CambiarEstadoTratamiento(
        Guid internamientoId, Guid tratamientoId, [FromBody] bool realizado)
    {
        return Ok(await sender.Send(new CambiarEstadoTratamientoCommand
        {
            InternamientoId = internamientoId,
            TratamientoId = tratamientoId,
            Realizado = realizado
        }));
    }

    [HttpPost("{id:guid}/evoluciones")]
    public async Task<ActionResult<InternamientoDto>> AgregarEvolucion(Guid id, AgregarEvolucionCommand command)
    {
        if (id != command.InternamientoId) return BadRequest("El id de la ruta no coincide con el del cuerpo.");
        return Ok(await sender.Send(command));
    }

    [HttpPost("{id:guid}/alta")]
    public async Task<ActionResult<InternamientoDto>> DarAlta(Guid id, DarAltaCommand command)
    {
        if (id != command.Id) return BadRequest("El id de la ruta no coincide con el del cuerpo.");
        return Ok(await sender.Send(command));
    }
}
