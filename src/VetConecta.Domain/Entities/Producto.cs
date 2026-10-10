using VetConecta.Domain.Common;

namespace VetConecta.Domain.Entities;

public class Producto : BaseEntity
{
    public string Nombre { get; set; } = default!;

    /// <summary>"Farmacia" | "Pet Shop". Texto plano, no enum: el espacio en
    /// "Pet Shop" no es un nombre de miembro C# valido.</summary>
    public string Categoria { get; set; } = default!;

    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; }
}
