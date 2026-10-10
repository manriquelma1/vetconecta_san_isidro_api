using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;

namespace VetConecta.Application.Productos.Queries.GetProductos;

/// <summary>Igual que buscarProductos(texto) en inventario.ts: sin texto, devuelve todo.</summary>
public record GetProductosQuery(string? Texto = null) : IRequest<List<ProductoDto>>;

public class GetProductosQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetProductosQuery, List<ProductoDto>>
{
    public async Task<List<ProductoDto>> Handle(GetProductosQuery request, CancellationToken cancellationToken)
    {
        var query = context.Productos.AsNoTracking().AsQueryable();

        var texto = request.Texto?.Trim();
        if (!string.IsNullOrEmpty(texto))
        {
            query = query.Where(p => EF.Functions.Like(p.Nombre, $"%{texto}%"));
        }

        return await query
            .OrderBy(p => p.Nombre)
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Categoria = p.Categoria,
                Precio = p.Precio,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo
            })
            .ToListAsync(cancellationToken);
    }
}
