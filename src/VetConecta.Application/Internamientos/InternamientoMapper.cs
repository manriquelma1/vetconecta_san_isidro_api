using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;

namespace VetConecta.Application.Internamientos;

/// <summary>Arma el InternamientoDto completo, con tratamientos y evoluciones ordenadas
/// igual que internamiento.ts (evoluciones de la mas reciente a la mas antigua).</summary>
public static class InternamientoMapper
{
    public static async Task<InternamientoDto> MapAsync(
        IApplicationDbContext context, Guid internamientoId, CancellationToken cancellationToken)
    {
        var internamiento = await context.Internamientos.AsNoTracking()
            .Include(i => i.Tratamientos)
            .Include(i => i.Evoluciones)
            .FirstAsync(i => i.Id == internamientoId, cancellationToken);

        return Map(internamiento);
    }

    public static InternamientoDto Map(Domain.Entities.Internamiento internamiento) => new()
    {
        Id = internamiento.Id,
        MascotaId = internamiento.MascotaId,
        Jaula = internamiento.Jaula,
        MotivoIngreso = internamiento.MotivoIngreso,
        FechaIngreso = internamiento.FechaIngreso.ToString("yyyy-MM-dd"),
        HoraIngreso = internamiento.HoraIngreso.ToString("HH:mm"),
        Estado = internamiento.Estado,
        FechaAlta = internamiento.FechaAlta?.ToString("yyyy-MM-dd"),
        Temperatura = internamiento.Temperatura,
        FrecuenciaCardiaca = internamiento.FrecuenciaCardiaca,
        FrecuenciaRespiratoria = internamiento.FrecuenciaRespiratoria,
        Tratamientos = internamiento.Tratamientos
            .Select(t => new TratamientoDto
            {
                Id = t.Id, Nombre = t.Nombre, Horario = t.Horario, Detalle = t.Detalle, Realizado = t.Realizado
            })
            .ToList(),
        Evoluciones = internamiento.Evoluciones
            .OrderByDescending(e => e.Fecha).ThenByDescending(e => e.Hora)
            .Select(e => new EvolucionDto
            {
                Id = e.Id,
                Fecha = e.Fecha.ToString("yyyy-MM-dd"),
                Hora = e.Hora.ToString("HH:mm"),
                Nota = e.Nota,
                Veterinario = e.Veterinario
            })
            .ToList()
    };
}
