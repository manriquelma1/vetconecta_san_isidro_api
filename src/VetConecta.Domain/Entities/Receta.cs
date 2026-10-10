using VetConecta.Domain.Common;

namespace VetConecta.Domain.Entities;

public class Receta : BaseEntity
{
    public Guid MascotaId { get; set; }
    public Mascota Mascota { get; set; } = default!;

    public string Veterinario { get; set; } = default!;
    public DateOnly FechaEmision { get; set; }
    public DateOnly FechaFin { get; set; }

    public ICollection<MedicamentoReceta> Medicamentos { get; set; } = new List<MedicamentoReceta>();
}

public class MedicamentoReceta : BaseEntity
{
    public Guid RecetaId { get; set; }
    public Receta Receta { get; set; } = default!;

    public string Nombre { get; set; } = default!;
    public string Dosis { get; set; } = default!;
    public string Duracion { get; set; } = default!;
}
