using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;

namespace VetConecta.Application.Historial.Commands.CrearVisita;

public record CrearVisitaCommand : IRequest<VisitaMedicaDto>
{
    public Guid MascotaId { get; init; }
    public string Fecha { get; init; } = default!;
    public string Hora { get; init; } = default!;
    public string Motivo { get; init; } = default!;
    public string? Descripcion { get; init; }
    public string Diagnostico { get; init; } = default!;
    public string Tratamiento { get; init; } = default!;
    public string Veterinario { get; init; } = default!;
}

/// <summary>Mismas reglas que el formulario reactivo de registro-atencion.ts.</summary>
public class CrearVisitaCommandValidator : AbstractValidator<CrearVisitaCommand>
{
    public CrearVisitaCommandValidator()
    {
        RuleFor(x => x.MascotaId).NotEmpty();
        RuleFor(x => x.Fecha).NotEmpty().Must(f => DateOnly.TryParse(f, out _)).WithMessage("La fecha no es válida.");
        RuleFor(x => x.Hora).NotEmpty().Must(h => TimeOnly.TryParse(h, out _)).WithMessage("La hora no es válida.");
        RuleFor(x => x.Motivo).NotEmpty();
        RuleFor(x => x.Diagnostico).NotEmpty().MinimumLength(5);
        RuleFor(x => x.Tratamiento).NotEmpty().MinimumLength(5);
        RuleFor(x => x.Veterinario).NotEmpty();
    }
}

/// <summary>Solo el personal veterinario registra atenciones (HU-08).</summary>
public class CrearVisitaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<CrearVisitaCommand, VisitaMedicaDto>
{
    public async Task<VisitaMedicaDto> Handle(CrearVisitaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede registrar una atención.");
        }

        var existeMascota = await context.Mascotas.AsNoTracking().AnyAsync(m => m.Id == request.MascotaId, cancellationToken);
        if (!existeMascota)
        {
            throw new ValidationException(
                [new FluentValidation.Results.ValidationFailure(nameof(request.MascotaId), "La mascota no existe.")]);
        }

        var visita = new VisitaMedica
        {
            MascotaId = request.MascotaId,
            Fecha = DateOnly.Parse(request.Fecha),
            Hora = TimeOnly.Parse(request.Hora),
            Motivo = request.Motivo.Trim(),
            Descripcion = request.Descripcion?.Trim(),
            Diagnostico = request.Diagnostico.Trim(),
            Tratamiento = request.Tratamiento.Trim(),
            Veterinario = request.Veterinario.Trim()
        };

        context.VisitasMedicas.Add(visita);
        await context.SaveChangesAsync(cancellationToken);

        return new VisitaMedicaDto
        {
            Id = visita.Id,
            MascotaId = visita.MascotaId,
            Fecha = visita.Fecha.ToString("yyyy-MM-dd"),
            Hora = visita.Hora.ToString("HH:mm"),
            Motivo = visita.Motivo,
            Descripcion = visita.Descripcion,
            Diagnostico = visita.Diagnostico,
            Tratamiento = visita.Tratamiento,
            Veterinario = visita.Veterinario
        };
    }
}
