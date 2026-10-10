namespace VetConecta.Application.Historial;

public record VacunaDto
{
    public Guid Id { get; init; }
    public Guid MascotaId { get; init; }
    public string Nombre { get; init; } = default!;
    public string FechaAplicacion { get; init; } = default!;
    public string ProximaDosis { get; init; } = default!;
    /// <summary>"Al día" | "Vencida", calculado contra la fecha de hoy.</summary>
    public string Estado { get; init; } = default!;
}

public record VisitaMedicaDto
{
    public Guid Id { get; init; }
    public Guid MascotaId { get; init; }
    public string Fecha { get; init; } = default!;
    public string Hora { get; init; } = default!;
    public string Motivo { get; init; } = default!;
    public string? Descripcion { get; init; }
    public string Diagnostico { get; init; } = default!;
    public string Tratamiento { get; init; } = default!;
    public string Veterinario { get; init; } = default!;
}
