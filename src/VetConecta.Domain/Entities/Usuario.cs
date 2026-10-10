using VetConecta.Domain.Common;
using VetConecta.Domain.Enums;

namespace VetConecta.Domain.Entities;

public class Usuario : BaseAuditableEntity
{
    public string Nombre { get; set; } = default!;
    public string Correo { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public RolUsuario Rol { get; set; }

    /// <summary>"correo" | "google" — igual que Usuario.proveedor en Angular.</summary>
    public string Proveedor { get; set; } = "correo";

    public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}
