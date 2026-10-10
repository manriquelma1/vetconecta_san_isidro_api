using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Enums;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Citas.Commands.CambiarEstadoCita;

public record CambiarEstadoCitaCommand : IRequest
{
    public Guid Id { get; init; }
    public EstadoCita Estado { get; init; }
}

public class CambiarEstadoCitaCommandValidator : AbstractValidator<CambiarEstadoCitaCommand>
{
    public CambiarEstadoCitaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Estado).IsInEnum();
    }
}

/// <summary>
/// Mismo permiso que cambiarEstado() en citas-service.ts:
/// - Personal: puede cambiar cualquier cita a cualquier estado.
/// - Propietario: solo puede cancelar SU PROPIA cita, y solo si esta Pendiente.
/// </summary>
public class CambiarEstadoCitaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<CambiarEstadoCitaCommand>
{
    public async Task Handle(CambiarEstadoCitaCommand request, CancellationToken cancellationToken)
    {
        var cita = await context.Citas.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("La cita no está disponible.");

        if (!currentUser.EsPersonal && (
                cita.PropietarioId != currentUser.UsuarioId ||
                cita.Estado != EstadoCita.Pendiente ||
                request.Estado != EstadoCita.Cancelada))
        {
            throw new ForbiddenAccessException("No tienes permiso para realizar este cambio.");
        }

        if (cita.Estado == EstadoCita.Cancelada && request.Estado != EstadoCita.Cancelada)
        {
            var ocupado = await context.Citas.AsNoTracking().AnyAsync(
                otra => otra.Id != cita.Id && otra.Fecha == cita.Fecha && otra.Hora == cita.Hora &&
                        otra.Estado != EstadoCita.Cancelada,
                cancellationToken);

            if (ocupado)
            {
                throw new ValidationException(
                    [new FluentValidation.Results.ValidationFailure(
                        nameof(request.Estado),
                        "El horario ya está ocupado por otra cita. No se puede reactivar.")]);
            }
        }

        cita.Estado = request.Estado;
        await context.SaveChangesAsync(cancellationToken);
    }
}
