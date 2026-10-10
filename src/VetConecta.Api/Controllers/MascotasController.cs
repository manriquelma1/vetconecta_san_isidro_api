using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Mascotas.Commands.CreateMascota;
using VetConecta.Application.Mascotas.Commands.DeleteMascota;
using VetConecta.Application.Mascotas.Commands.UpdateMascota;
using VetConecta.Application.Mascotas.Queries.GetMascotas;

namespace VetConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MascotasController(ISender sender) : ControllerBase
{
    /// <summary>Un propietario ve solo las suyas; el personal, todas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<MascotaDto>>> GetMascotas()
        => Ok(await sender.Send(new GetMascotasQuery()));

    [HttpPost]
    public async Task<ActionResult<MascotaDto>> CreateMascota(CreateMascotaCommand command)
    {
        var mascota = await sender.Send(command);
        return CreatedAtAction(nameof(GetMascotas), new { id = mascota.Id }, mascota);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMascota(Guid id, UpdateMascotaCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("El id de la ruta no coincide con el del cuerpo.");
        }

        await sender.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Un propietario puede eliminar sus propias mascotas; el personal, cualquiera.
    /// El control fino de "de quien es" vive en DeleteMascotaCommandHandler, no aqui.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMascota(Guid id)
    {
        await sender.Send(new DeleteMascotaCommand(id));
        return NoContent();
    }
}
