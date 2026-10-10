using VetConecta.Domain.Enums;

namespace VetConecta.Application.Citas;

public record CitaDto
{
    public Guid Id { get; init; }
    public Guid MascotaId { get; init; }
    public string MascotaNombre { get; init; } = default!;
    public string Servicio { get; init; } = default!;
    public string MotivoConsulta { get; init; } = default!;
    /// <summary>Formato YYYY-MM-DD, igual que el string que ya maneja Angular.</summary>
    public string Fecha { get; init; } = default!;
    /// <summary>Formato HH:mm.</summary>
    public string Hora { get; init; } = default!;
    public EstadoCita Estado { get; init; }
    public string PropietarioNombre { get; init; } = default!;
    public string PropietarioCorreo { get; init; } = default!;
    public string CreadaEn { get; init; } = default!;
}
