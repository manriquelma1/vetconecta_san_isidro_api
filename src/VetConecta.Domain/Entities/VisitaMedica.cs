using VetConecta.Domain.Common;

namespace VetConecta.Domain.Entities;

public class VisitaMedica : BaseEntity
{
    public Guid MascotaId { get; set; }
    public Mascota Mascota { get; set; } = default!;

    public DateOnly Fecha { get; set; }
    public TimeOnly Hora { get; set; }
    public string Motivo { get; set; } = default!;
    public string? Descripcion { get; set; }
    public string Diagnostico { get; set; } = default!;
    public string Tratamiento { get; set; } = default!;
    public string Veterinario { get; set; } = default!;
}
