using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Productos.Commands.VenderProducto;

public record VenderProductoCommand : IRequest<ProductoDto>
{
    public Guid Id { get; init; }
    public int Cantidad { get; init; } = 1;
}

public class VenderProductoCommandValidator : AbstractValidator<VenderProductoCommand>
{
    public VenderProductoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Cantidad).GreaterThan(0);
    }
}

/// <summary>Solo personal de farmacia (HU-10): descuenta stock al confirmar la venta.</summary>
public class VenderProductoCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<VenderProductoCommand, ProductoDto>
{
    public async Task<ProductoDto> Handle(VenderProductoCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal de farmacia puede registrar ventas.");
        }

        var producto = await context.Productos.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Producto), request.Id);

        if (producto.Stock < request.Cantidad)
        {
            throw new ValidationException(
                [new FluentValidation.Results.ValidationFailure(nameof(request.Cantidad), "No hay stock disponible.")]);
        }

        producto.Stock -= request.Cantidad;
        await context.SaveChangesAsync(cancellationToken);

        return new ProductoDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Categoria = producto.Categoria,
            Precio = producto.Precio,
            Stock = producto.Stock,
            StockMinimo = producto.StockMinimo
        };
    }
}
