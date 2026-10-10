using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Citas;
using VetConecta.Application.Citas.Commands.CambiarEstadoCita;
using VetConecta.Application.Citas.Commands.CrearCita;
using VetConecta.Application.Citas.Queries.GetCitas;
using VetConecta.Application.Citas.Queries.GetHorasOcupadas;

namespace VetConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CitasController(ISender sender) : ControllerBase
{
    /// <summary>Personal ve todas; propietario, solo las suyas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<CitaDto>>> GetCitas()
        => Ok(await sender.Send(new GetCitasQuery()));

    /// <summary>Horarios ocupados en una fecha, compartidos por toda la clinica (no por dueño).</summary>
    [HttpGet("horas-ocupadas")]
    public async Task<ActionResult<List<string>>> GetHorasOcupadas([FromQuery] string fecha)
        => Ok(await sender.Send(new GetHorasOcupadasQuery(fecha)));

    [HttpPost]
    public async Task<ActionResult<CitaDto>> CrearCita(CrearCitaCommand command)
        => Ok(await sender.Send(command));

    [HttpPatch("{id:guid}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, CambiarEstadoCitaCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("El id de la ruta no coincide con el del cuerpo.");
        }

        await sender.Send(command);
        return NoContent();
    }
}
