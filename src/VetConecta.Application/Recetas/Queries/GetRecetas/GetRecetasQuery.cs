using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;

namespace VetConecta.Application.Recetas.Queries.GetRecetas;

/// <summary>
/// El personal ve todas las recetas; un propietario, solo las de sus mascotas.
/// (Esto no estaba filtrado asi en la version localStorage de Angular: ahi cualquier
/// propietario podia ver las recetas de cualquier mascota.)
/// </summary>
public record GetRecetasQuery : IRequest<List<RecetaDto>>;

public class GetRecetasQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<GetRecetasQuery, List<RecetaDto>>
{
    public async Task<List<RecetaDto>> Handle(GetRecetasQuery request, CancellationToken cancellationToken)
    {
        var query = context.Recetas.AsNoTracking().AsQueryable();

        if (!currentUser.EsPersonal)
        {
            query = query.Where(r => r.Mascota.PropietarioId == currentUser.UsuarioId);
        }

        var recetas = await query
            .OrderByDescending(r => r.FechaEmision)
            .Select(r => new
            {
                r.Id,
                r.MascotaId,
                r.Veterinario,
                r.FechaEmision,
                r.FechaFin,
                Medicamentos = r.Medicamentos.Select(m => new { m.Id, m.Nombre, m.Dosis, m.Duracion }).ToList()
            })
            .ToListAsync(cancellationToken);

        return recetas.Select(r => new RecetaDto
        {
            Id = r.Id,
            MascotaId = r.MascotaId,
            Veterinario = r.Veterinario,
            FechaEmision = r.FechaEmision.ToString("yyyy-MM-dd"),
            FechaFin = r.FechaFin.ToString("yyyy-MM-dd"),
            Medicamentos = r.Medicamentos
                .Select(m => new MedicamentoDto { Id = m.Id, Nombre = m.Nombre, Dosis = m.Dosis, Duracion = m.Duracion })
                .ToList()
        }).ToList();
    }
}
