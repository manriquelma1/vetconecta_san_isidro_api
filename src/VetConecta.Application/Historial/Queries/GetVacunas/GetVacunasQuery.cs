using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;

namespace VetConecta.Application.Historial.Queries.GetVacunas;

/// <summary>
/// Vacunas de TODAS las mascotas visibles para el usuario actual (personal: todas;
/// propietario: solo las suyas). Lo usa el panel de recordatorios para listar
/// refuerzos proximos sin tener que pedir el historial mascota por mascota.
/// </summary>
public record GetVacunasQuery : IRequest<List<VacunaDto>>;

public class GetVacunasQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<GetVacunasQuery, List<VacunaDto>>
{
    public async Task<List<VacunaDto>> Handle(GetVacunasQuery request, CancellationToken cancellationToken)
    {
        var query = context.Vacunas.AsNoTracking().AsQueryable();

        if (!currentUser.EsPersonal)
        {
            query = query.Where(v => v.Mascota.PropietarioId == currentUser.UsuarioId);
        }

        var vacunas = await query
            .Select(v => new { v.Id, v.MascotaId, v.Nombre, v.FechaAplicacion, v.ProximaDosis })
            .ToListAsync(cancellationToken);

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

        return vacunas.Select(v => new VacunaDto
        {
            Id = v.Id,
            MascotaId = v.MascotaId,
            Nombre = v.Nombre,
            FechaAplicacion = v.FechaAplicacion.ToString("yyyy-MM-dd"),
            ProximaDosis = v.ProximaDosis.ToString("yyyy-MM-dd"),
            Estado = v.ProximaDosis < hoy ? "Vencida" : "Al día"
        }).ToList();
    }
}
