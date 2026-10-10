namespace VetConecta.Application.Recetas;

public record MedicamentoDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = default!;
    public string Dosis { get; init; } = default!;
    public string Duracion { get; init; } = default!;
}

public record RecetaDto
{
    public Guid Id { get; init; }
    public Guid MascotaId { get; init; }
    public string Veterinario { get; init; } = default!;
    /// <summary>YYYY-MM-DD</summary>
    public string FechaEmision { get; init; } = default!;
    public string FechaFin { get; init; } = default!;
    public List<MedicamentoDto> Medicamentos { get; init; } = [];
}
