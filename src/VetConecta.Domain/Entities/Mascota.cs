using VetConecta.Domain.Common;

namespace VetConecta.Domain.Entities;

public class Mascota : BaseAuditableEntity
{
    public string Nombre { get; set; } = default!;
    public string Especie { get; set; } = default!;
    public string Raza { get; set; } = default!;
    public int Edad { get; set; }
    public decimal Peso { get; set; }
    public string? Alergias { get; set; }
    public string? FotoUrl { get; set; }

    /// <summary>
    /// No existe hoy en el modelo Angular (mascotas globales). Se agrega para que
    /// cada propietario solo vea y edite las suyas una vez haya JWT real.
    /// </summary>
    public Guid PropietarioId { get; set; }
    public Usuario Propietario { get; set; } = default!;
}
