using VetConecta.Domain.Common;
using VetConecta.Domain.Enums;

namespace VetConecta.Domain.Entities;

public class Internamiento : BaseEntity
{
    public Guid MascotaId { get; set; }
    public Mascota Mascota { get; set; } = default!;

    public string Jaula { get; set; } = default!;
    public string MotivoIngreso { get; set; } = default!;
    public DateOnly FechaIngreso { get; set; }
    public TimeOnly HoraIngreso { get; set; }
    public EstadoInternamiento Estado { get; set; } = EstadoInternamiento.Internado;
    public DateOnly? FechaAlta { get; set; }

    public decimal Temperatura { get; set; }
    public int FrecuenciaCardiaca { get; set; }
    public int FrecuenciaRespiratoria { get; set; }

    public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
    public ICollection<EvolucionInternamiento> Evoluciones { get; set; } = new List<EvolucionInternamiento>();
}

public class Tratamiento : BaseEntity
{
    public Guid InternamientoId { get; set; }
    public Internamiento Internamiento { get; set; } = default!;

    public string Nombre { get; set; } = default!;
    public string Horario { get; set; } = default!;
    public string Detalle { get; set; } = default!;
    public bool Realizado { get; set; }
}

public class EvolucionInternamiento : BaseEntity
{
    public Guid InternamientoId { get; set; }
    public Internamiento Internamiento { get; set; } = default!;

    public DateOnly Fecha { get; set; }
    public TimeOnly Hora { get; set; }
    public string Nota { get; set; } = default!;
    public string Veterinario { get; set; } = default!;
}
