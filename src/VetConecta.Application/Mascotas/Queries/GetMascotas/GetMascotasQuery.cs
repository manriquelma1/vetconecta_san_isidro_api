using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;

namespace VetConecta.Application.Mascotas.Queries.GetMascotas;

public record MascotaDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = default!;
    public string Especie { get; init; } = default!;
    public string Raza { get; init; } = default!;
    public int Edad { get; init; }
    public decimal Peso { get; init; }
    public string? Alergias { get; init; }
    public string? Foto { get; init; }
}

public record GetMascotasQuery : IRequest<List<MascotaDto>>;

public class GetMascotasQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<GetMascotasQuery, List<MascotaDto>>
{
    public async Task<List<MascotaDto>> Handle(GetMascotasQuery request, CancellationToken cancellationToken)
    {
        var query = context.Mascotas.AsNoTracking().AsQueryable();

        // El personal ve todas las mascotas; un propietario solo las suyas.
        if (!currentUser.EsPersonal)
        {
            query = query.Where(m => m.PropietarioId == currentUser.UsuarioId);
        }

        return await query
            .OrderBy(m => m.Nombre)
            .Select(m => new MascotaDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Especie = m.Especie,
                Raza = m.Raza,
                Edad = m.Edad,
                Peso = m.Peso,
                Alergias = m.Alergias,
                Foto = m.FotoUrl
            })
            .ToListAsync(cancellationToken);
    }
}
