using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Enums;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Internamientos.Commands.DarAlta;

public record DarAltaCommand : IRequest<InternamientoDto>
{
    public Guid Id { get; init; }
    public string FechaAlta { get; init; } = default!;
}

public class DarAltaCommandValidator : AbstractValidator<DarAltaCommand>
{
    public DarAltaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FechaAlta).NotEmpty().Must(f => DateOnly.TryParse(f, out _)).WithMessage("La fecha de alta no es válida.");
    }
}

/// <summary>Da de alta a la mascota (HU-05: el estado pasa a "Alta" con su fecha).</summary>
public class DarAltaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<DarAltaCommand, InternamientoDto>
{
    public async Task<InternamientoDto> Handle(DarAltaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede dar de alta.");
        }

        var internamiento = await context.Internamientos.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Internamiento), request.Id);

        internamiento.Estado = EstadoInternamiento.Alta;
        internamiento.FechaAlta = DateOnly.Parse(request.FechaAlta);

        await context.SaveChangesAsync(cancellationToken);

        return await InternamientoMapper.MapAsync(context, internamiento.Id, cancellationToken);
    }
}
