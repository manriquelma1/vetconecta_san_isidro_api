using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;

namespace VetConecta.Application.Citas.Queries.GetCitas;

/// <summary>El personal ve todas las citas; un propietario, solo las suyas.</summary>
public record GetCitasQuery : IRequest<List<CitaDto>>;

public class GetCitasQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<GetCitasQuery, List<CitaDto>>
{
    public async Task<List<CitaDto>> Handle(GetCitasQuery request, CancellationToken cancellationToken)
    {
        var query = context.Citas.AsNoTracking().AsQueryable();

        if (!currentUser.EsPersonal)
        {
            query = query.Where(c => c.PropietarioId == currentUser.UsuarioId);
        }

        // Paso 1: traer las entidades con DateOnly/TimeOnly crudos (traduce bien a SQL).
        var citas = await query
            .OrderBy(c => c.Fecha).ThenBy(c => c.Hora)
            .Select(c => new
            {
                c.Id,
                c.MascotaId,
                MascotaNombre = c.Mascota.Nombre,
                c.Servicio,
                c.MotivoConsulta,
                c.Fecha,
                c.Hora,
                c.Estado,
                PropietarioNombre = c.Propietario.Nombre,
                PropietarioCorreo = c.Propietario.Correo,
                c.CreatedAt
            })
            .ToListAsync(cancellationToken);

        // Paso 2: formatear a los strings "YYYY-MM-DD" / "HH:mm" que espera Angular, en memoria.
        return citas.Select(c => new CitaDto
        {
            Id = c.Id,
            MascotaId = c.MascotaId,
            MascotaNombre = c.MascotaNombre,
            Servicio = c.Servicio,
            MotivoConsulta = c.MotivoConsulta,
            Fecha = c.Fecha.ToString("yyyy-MM-dd"),
            Hora = c.Hora.ToString("HH:mm"),
            Estado = c.Estado,
            PropietarioNombre = c.PropietarioNombre,
            PropietarioCorreo = c.PropietarioCorreo,
            CreadaEn = c.CreatedAt.ToString("o")
        }).ToList();
    }
}
