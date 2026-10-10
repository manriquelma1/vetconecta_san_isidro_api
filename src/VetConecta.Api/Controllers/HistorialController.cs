using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Historial;
using VetConecta.Application.Historial.Commands.CrearVisita;
using VetConecta.Application.Historial.Queries.GetHistorial;
using VetConecta.Application.Historial.Queries.GetVacunas;

namespace VetConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistorialController(ISender sender) : ControllerBase
{
    /// <summary>Vacunas y visitas de una mascota. Propietario: solo la suya.</summary>
    [HttpGet("mascota/{mascotaId:guid}")]
    public async Task<ActionResult<HistorialDto>> GetHistorial(Guid mascotaId)
        => Ok(await sender.Send(new GetHistorialPorMascotaQuery(mascotaId)));

    /// <summary>Vacunas de todas las mascotas visibles (para el panel de recordatorios).</summary>
    [HttpGet("vacunas")]
    public async Task<ActionResult<List<VacunaDto>>> GetVacunas()
        => Ok(await sender.Send(new GetVacunasQuery()));

    /// <summary>Registrar una atencion medica. Solo personal (HU-08).</summary>
    [HttpPost("visitas")]
    public async Task<ActionResult<VisitaMedicaDto>> CrearVisita(CrearVisitaCommand command)
        => Ok(await sender.Send(command));
}
