using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Internamientos.Commands.AgregarEvolucion;

public record AgregarEvolucionCommand : IRequest<InternamientoDto>
{
    public Guid InternamientoId { get; init; }
    public string Fecha { get; init; } = default!;
    public string Hora { get; init; } = default!;
    public string Nota { get; init; } = default!;
    public string Veterinario { get; init; } = default!;
}

public class AgregarEvolucionCommandValidator : AbstractValidator<AgregarEvolucionCommand>
{
    public AgregarEvolucionCommandValidator()
    {
        RuleFor(x => x.InternamientoId).NotEmpty();
        RuleFor(x => x.Fecha).NotEmpty().Must(f => DateOnly.TryParse(f, out _)).WithMessage("La fecha no es válida.");
        RuleFor(x => x.Hora).NotEmpty().Must(h => TimeOnly.TryParse(h, out _)).WithMessage("La hora no es válida.");
        RuleFor(x => x.Nota).NotEmpty().WithMessage("Ingresa la nota de evolución.");
        RuleFor(x => x.Veterinario).NotEmpty().WithMessage("Ingresa el nombre del veterinario.");
    }
}

public class AgregarEvolucionCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<AgregarEvolucionCommand, InternamientoDto>
{
    public async Task<InternamientoDto> Handle(AgregarEvolucionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede registrar una evolución.");
        }

        var existe = await context.Internamientos.AsNoTracking().AnyAsync(i => i.Id == request.InternamientoId, cancellationToken);
        if (!existe)
        {
            throw new NotFoundException(nameof(Domain.Entities.Internamiento), request.InternamientoId);
        }

        context.Evoluciones.Add(new EvolucionInternamiento
        {
            InternamientoId = request.InternamientoId,
            Fecha = DateOnly.Parse(request.Fecha),
            Hora = TimeOnly.Parse(request.Hora),
            Nota = request.Nota.Trim(),
            Veterinario = request.Veterinario.Trim()
        });

        await context.SaveChangesAsync(cancellationToken);

        return await InternamientoMapper.MapAsync(context, request.InternamientoId, cancellationToken);
    }
}
