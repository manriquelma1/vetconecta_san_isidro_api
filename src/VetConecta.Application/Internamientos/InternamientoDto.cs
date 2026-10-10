using VetConecta.Domain.Enums;

namespace VetConecta.Application.Internamientos;

public record TratamientoDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = default!;
    public string Horario { get; init; } = default!;
    public string Detalle { get; init; } = default!;
    public bool Realizado { get; init; }
}

public record EvolucionDto
{
    public Guid Id { get; init; }
    public string Fecha { get; init; } = default!;
    public string Hora { get; init; } = default!;
    public string Nota { get; init; } = default!;
    public string Veterinario { get; init; } = default!;
}

public record InternamientoDto
{
    public Guid Id { get; init; }
    public Guid MascotaId { get; init; }
    public string Jaula { get; init; } = default!;
    public string MotivoIngreso { get; init; } = default!;
    public string FechaIngreso { get; init; } = default!;
    public string HoraIngreso { get; init; } = default!;
    public EstadoInternamiento Estado { get; init; }
    public string? FechaAlta { get; init; }
    public decimal Temperatura { get; init; }
    public int FrecuenciaCardiaca { get; init; }
    public int FrecuenciaRespiratoria { get; init; }
    public List<TratamientoDto> Tratamientos { get; init; } = [];
    public List<EvolucionDto> Evoluciones { get; init; } = [];
}
