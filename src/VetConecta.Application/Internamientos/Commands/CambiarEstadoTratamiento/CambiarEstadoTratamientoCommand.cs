using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Internamientos.Commands.CambiarEstadoTratamiento;

public record CambiarEstadoTratamientoCommand : IRequest<InternamientoDto>
{
    public Guid InternamientoId { get; init; }
    public Guid TratamientoId { get; init; }
    public bool Realizado { get; init; }
}

public class CambiarEstadoTratamientoCommandValidator : AbstractValidator<CambiarEstadoTratamientoCommand>
{
    public CambiarEstadoTratamientoCommandValidator()
    {
        RuleFor(x => x.InternamientoId).NotEmpty();
        RuleFor(x => x.TratamientoId).NotEmpty();
    }
}

public class CambiarEstadoTratamientoCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<CambiarEstadoTratamientoCommand, InternamientoDto>
{
    public async Task<InternamientoDto> Handle(CambiarEstadoTratamientoCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede marcar los tratamientos.");
        }

        var tratamiento = await context.Tratamientos.FirstOrDefaultAsync(
            t => t.Id == request.TratamientoId && t.InternamientoId == request.InternamientoId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Tratamiento), request.TratamientoId);

        tratamiento.Realizado = request.Realizado;
        await context.SaveChangesAsync(cancellationToken);

        return await InternamientoMapper.MapAsync(context, request.InternamientoId, cancellationToken);
    }
}
