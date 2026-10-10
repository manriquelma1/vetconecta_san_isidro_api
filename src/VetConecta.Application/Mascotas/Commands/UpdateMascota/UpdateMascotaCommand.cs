using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Mascotas.Commands.UpdateMascota;

public record UpdateMascotaCommand : IRequest
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = default!;
    public string Especie { get; init; } = default!;
    public string Raza { get; init; } = default!;
    public int Edad { get; init; }
    public decimal Peso { get; init; }
    public string? Alergias { get; init; }
}

public class UpdateMascotaCommandValidator : AbstractValidator<UpdateMascotaCommand>
{
    public UpdateMascotaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Especie).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Raza).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Edad).InclusiveBetween(0, 50);
        RuleFor(x => x.Peso).GreaterThan(0);
        RuleFor(x => x.Alergias).MaximumLength(300);
    }
}

public class UpdateMascotaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<UpdateMascotaCommand>
{
    public async Task Handle(UpdateMascotaCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Mascota), request.Id);

        // Un propietario solo puede editar sus propias mascotas; el personal, cualquiera.
        if (!currentUser.EsPersonal && entity.PropietarioId != currentUser.UsuarioId)
        {
            throw new ForbiddenAccessException("No puedes editar la mascota de otro propietario.");
        }

        entity.Nombre = request.Nombre;
        entity.Especie = request.Especie;
        entity.Raza = request.Raza;
        entity.Edad = request.Edad;
        entity.Peso = request.Peso;
        entity.Alergias = request.Alergias;
        entity.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
