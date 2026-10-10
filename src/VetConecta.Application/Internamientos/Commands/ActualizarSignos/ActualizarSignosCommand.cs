using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Internamientos.Commands.ActualizarSignos;

public record ActualizarSignosCommand : IRequest<InternamientoDto>
{
    public Guid Id { get; init; }
    public decimal Temperatura { get; init; }
    public int FrecuenciaCardiaca { get; init; }
    public int FrecuenciaRespiratoria { get; init; }
}

public class ActualizarSignosCommandValidator : AbstractValidator<ActualizarSignosCommand>
{
    public ActualizarSignosCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Temperatura).GreaterThan(0).WithMessage("Ingresa una temperatura válida.");
        RuleFor(x => x.FrecuenciaCardiaca).GreaterThan(0).WithMessage("Ingresa una frecuencia cardíaca válida.");
        RuleFor(x => x.FrecuenciaRespiratoria).GreaterThan(0).WithMessage("Ingresa una frecuencia respiratoria válida.");
    }
}

/// <summary>Solo personal: quien actualiza signos vitales es la clinica, no el dueño.</summary>
public class ActualizarSignosCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<ActualizarSignosCommand, InternamientoDto>
{
    public async Task<InternamientoDto> Handle(ActualizarSignosCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede actualizar los signos vitales.");
        }

        var internamiento = await context.Internamientos.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Internamiento), request.Id);

        internamiento.Temperatura = request.Temperatura;
        internamiento.FrecuenciaCardiaca = request.FrecuenciaCardiaca;
        internamiento.FrecuenciaRespiratoria = request.FrecuenciaRespiratoria;

        await context.SaveChangesAsync(cancellationToken);

        return await InternamientoMapper.MapAsync(context, internamiento.Id, cancellationToken);
    }
}
