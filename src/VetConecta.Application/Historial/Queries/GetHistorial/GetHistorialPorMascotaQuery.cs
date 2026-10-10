using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Historial.Queries.GetHistorial;

public record HistorialDto
{
    public List<VacunaDto> Vacunas { get; init; } = [];
    public List<VisitaMedicaDto> Visitas { get; init; } = [];
}

/// <summary>
/// Igual que vacunasDeMascota()/visitasDeMascota() en historial-service.ts, juntos
/// en una sola llamada. Un propietario solo puede pedir el historial de su mascota.
/// </summary>
public record GetHistorialPorMascotaQuery(Guid MascotaId) : IRequest<HistorialDto>;

public class GetHistorialPorMascotaQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<GetHistorialPorMascotaQuery, HistorialDto>
{
    public async Task<HistorialDto> Handle(GetHistorialPorMascotaQuery request, CancellationToken cancellationToken)
    {
        var mascota = await context.Mascotas.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.MascotaId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Mascota), request.MascotaId);

        if (!currentUser.EsPersonal && mascota.PropietarioId != currentUser.UsuarioId)
        {
            throw new ForbiddenAccessException("No puedes ver el historial de la mascota de otro propietario.");
        }

        var vacunas = await context.Vacunas.AsNoTracking()
            .Where(v => v.MascotaId == request.MascotaId)
            .Select(v => new { v.Id, v.MascotaId, v.Nombre, v.FechaAplicacion, v.ProximaDosis })
            .ToListAsync(cancellationToken);

        var visitas = await context.VisitasMedicas.AsNoTracking()
            .Where(v => v.MascotaId == request.MascotaId)
            .OrderByDescending(v => v.Fecha).ThenByDescending(v => v.Hora)
            .Select(v => new
            {
                v.Id,
                v.MascotaId,
                v.Fecha,
                v.Hora,
                v.Motivo,
                v.Descripcion,
                v.Diagnostico,
                v.Tratamiento,
                v.Veterinario
            })
            .ToListAsync(cancellationToken);

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

        return new HistorialDto
        {
            Vacunas = vacunas.Select(v => new VacunaDto
            {
                Id = v.Id,
                MascotaId = v.MascotaId,
                Nombre = v.Nombre,
                FechaAplicacion = v.FechaAplicacion.ToString("yyyy-MM-dd"),
                ProximaDosis = v.ProximaDosis.ToString("yyyy-MM-dd"),
                Estado = v.ProximaDosis < hoy ? "Vencida" : "Al día"
            }).ToList(),
            Visitas = visitas.Select(v => new VisitaMedicaDto
            {
                Id = v.Id,
                MascotaId = v.MascotaId,
                Fecha = v.Fecha.ToString("yyyy-MM-dd"),
                Hora = v.Hora.ToString("HH:mm"),
                Motivo = v.Motivo,
                Descripcion = v.Descripcion,
                Diagnostico = v.Diagnostico,
                Tratamiento = v.Tratamiento,
                Veterinario = v.Veterinario
            }).ToList()
        };
    }
}
