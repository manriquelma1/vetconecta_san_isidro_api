using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;
using VetConecta.Domain.Enums;

namespace VetConecta.Application.Citas.Commands.CrearCita;

public record CrearCitaCommand : IRequest<CitaDto>
{
    public Guid MascotaId { get; init; }
    public string Servicio { get; init; } = default!;
    public string MotivoConsulta { get; init; } = default!;
    /// <summary>YYYY-MM-DD</summary>
    public string Fecha { get; init; } = default!;
    /// <summary>HH:mm</summary>
    public string Hora { get; init; } = default!;
}

public class CrearCitaCommandValidator : AbstractValidator<CrearCitaCommand>
{
    public CrearCitaCommandValidator()
    {
        RuleFor(x => x.MascotaId).NotEmpty();
        RuleFor(x => x.Servicio).NotEmpty().MaximumLength(100);
        RuleFor(x => x.MotivoConsulta).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Fecha).NotEmpty()
            .Must(f => DateOnly.TryParse(f, out _)).WithMessage("La fecha no es válida.");
        RuleFor(x => x.Hora).NotEmpty()
            .Must(h => TimeOnly.TryParse(h, out _)).WithMessage("La hora no es válida.");
    }
}

public class CrearCitaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<CrearCitaCommand, CitaDto>
{
    public async Task<CitaDto> Handle(CrearCitaCommand request, CancellationToken cancellationToken)
    {
        // Solo un propietario agenda citas para si mismo (igual que el Angular: usuario.rol !== 'propietario' -> null).
        if (currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo los propietarios pueden agendar citas.");
        }

        var usuarioId = currentUser.UsuarioId
            ?? throw new UnauthorizedAccessException("No hay una sesión activa.");

        var mascota = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == request.MascotaId, cancellationToken)
            ?? throw new ValidationException(
                [new FluentValidation.Results.ValidationFailure(nameof(request.MascotaId), "La mascota no existe.")]);

        var usuario = await context.Usuarios.FirstAsync(u => u.Id == usuarioId, cancellationToken);

        var fecha = DateOnly.Parse(request.Fecha);
        var hora = TimeOnly.Parse(request.Hora);

        // Igual que horasOcupadas(fecha) en Angular: mira TODAS las citas de esa fecha,
        // no solo las del propietario actual, porque el horario es compartido por la clinica.
        var ocupado = await context.Citas.AsNoTracking().AnyAsync(
            c => c.Fecha == fecha && c.Hora == hora && c.Estado != EstadoCita.Cancelada,
            cancellationToken);

        if (ocupado)
        {
            throw new ValidationException(
                [new FluentValidation.Results.ValidationFailure(nameof(request.Hora), "El horario ya está ocupado.")]);
        }

        var cita = new Cita
        {
            MascotaId = mascota.Id,
            Servicio = request.Servicio.Trim(),
            MotivoConsulta = request.MotivoConsulta.Trim(),
            Fecha = fecha,
            Hora = hora,
            Estado = EstadoCita.Pendiente,
            PropietarioId = usuario.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.Citas.Add(cita);
        await context.SaveChangesAsync(cancellationToken);

        return new CitaDto
        {
            Id = cita.Id,
            MascotaId = mascota.Id,
            MascotaNombre = mascota.Nombre,
            Servicio = cita.Servicio,
            MotivoConsulta = cita.MotivoConsulta,
            Fecha = cita.Fecha.ToString("yyyy-MM-dd"),
            Hora = cita.Hora.ToString("HH:mm"),
            Estado = cita.Estado,
            PropietarioNombre = usuario.Nombre,
            PropietarioCorreo = usuario.Correo,
            CreadaEn = cita.CreatedAt.ToString("o")
        };
    }
}
