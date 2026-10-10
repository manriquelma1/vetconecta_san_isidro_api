using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Enums;

namespace VetConecta.Application.Citas.Queries.GetHorasOcupadas;

/// <summary>
/// Igual que horasOcupadas(fecha) en citas-service.ts: mira TODAS las citas de la
/// fecha, sin filtrar por propietario, porque el horario lo comparte toda la clinica.
/// </summary>
public record GetHorasOcupadasQuery(string Fecha) : IRequest<List<string>>;

public class GetHorasOcupadasQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetHorasOcupadasQuery, List<string>>
{
    public async Task<List<string>> Handle(GetHorasOcupadasQuery request, CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParse(request.Fecha, out var fecha))
        {
            return [];
        }

        var horas = await context.Citas.AsNoTracking()
            .Where(c => c.Fecha == fecha && c.Estado != EstadoCita.Cancelada)
            .Select(c => c.Hora)
            .ToListAsync(cancellationToken);

        return horas.Select(h => h.ToString("HH:mm")).ToList();
    }
}
