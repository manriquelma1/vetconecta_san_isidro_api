using VetConecta.Domain.Common;
using VetConecta.Domain.Enums;

namespace VetConecta.Domain.Entities;

public class Cita : BaseEntity
{
    public Guid MascotaId { get; set; }
    public Mascota Mascota { get; set; } = default!;

    public string Servicio { get; set; } = default!;
    public string MotivoConsulta { get; set; } = default!;
    public DateOnly Fecha { get; set; }
    public TimeOnly Hora { get; set; }
    public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

    public Guid PropietarioId { get; set; }
    public Usuario Propietario { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}
