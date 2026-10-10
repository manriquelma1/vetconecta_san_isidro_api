using VetConecta.Domain.Common;

namespace VetConecta.Domain.Entities;

public class Vacuna : BaseEntity
{
    public Guid MascotaId { get; set; }
    public Mascota Mascota { get; set; } = default!;

    public string Nombre { get; set; } = default!;
    public DateOnly FechaAplicacion { get; set; }
    public DateOnly ProximaDosis { get; set; }

    // "Al día" / "Vencida" no se guarda: se calcula en el DTO comparando
    // ProximaDosis con la fecha de hoy, para que nunca quede desactualizado.
}
