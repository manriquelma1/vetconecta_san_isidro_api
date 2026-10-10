namespace VetConecta.Application.Productos;

public record ProductoDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = default!;
    public string Categoria { get; init; } = default!;
    public decimal Precio { get; init; }
    public int Stock { get; init; }
    public int StockMinimo { get; init; }
}
