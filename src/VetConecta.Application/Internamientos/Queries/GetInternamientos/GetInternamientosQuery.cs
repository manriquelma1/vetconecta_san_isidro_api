using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;

namespace VetConecta.Application.Internamientos.Queries.GetInternamientos;

/// <summary>Personal ve todos los internamientos; propietario, solo los de sus mascotas (HU-05).</summary>
public record GetInternamientosQuery : IRequest<List<InternamientoDto>>;

public class GetInternamientosQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<GetInternamientosQuery, List<InternamientoDto>>
{
    public async Task<List<InternamientoDto>> Handle(GetInternamientosQuery request, CancellationToken cancellationToken)
    {
        var query = context.Internamientos.AsNoTracking()
            .Include(i => i.Tratamientos)
            .Include(i => i.Evoluciones)
            .AsQueryable();

        if (!currentUser.EsPersonal)
        {
            query = query.Where(i => i.Mascota.PropietarioId == currentUser.UsuarioId);
        }

        var internamientos = await query.ToListAsync(cancellationToken);

        return internamientos.Select(InternamientoMapper.Map).ToList();
    }
}
