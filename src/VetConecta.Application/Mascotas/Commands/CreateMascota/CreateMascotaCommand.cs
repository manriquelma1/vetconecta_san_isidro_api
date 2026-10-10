using FluentValidation;
using MediatR;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Application.Mascotas.Queries.GetMascotas;
using VetConecta.Domain.Entities;

namespace VetConecta.Application.Mascotas.Commands.CreateMascota;

public record CreateMascotaCommand : IRequest<MascotaDto>
{
    public string Nombre { get; init; } = default!;
    public string Especie { get; init; } = default!;
    public string Raza { get; init; } = default!;
    public int Edad { get; init; }
    public decimal Peso { get; init; }
    public string? Alergias { get; init; }
}

public class CreateMascotaCommandValidator : AbstractValidator<CreateMascotaCommand>
{
    public CreateMascotaCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Especie).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Raza).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Edad).InclusiveBetween(0, 50);
        RuleFor(x => x.Peso).GreaterThan(0);
        RuleFor(x => x.Alergias).MaximumLength(300);
    }
}

public class CreateMascotaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<CreateMascotaCommand, MascotaDto>
{
    public async Task<MascotaDto> Handle(CreateMascotaCommand request, CancellationToken cancellationToken)
    {
        var propietarioId = currentUser.UsuarioId
            ?? throw new UnauthorizedAccessException("No hay una sesión activa.");

        var entity = new Mascota
        {
            Nombre = request.Nombre,
            Especie = request.Especie,
            Raza = request.Raza,
            Edad = request.Edad,
            Peso = request.Peso,
            Alergias = request.Alergias,
            PropietarioId = propietarioId,
            CreatedAt = DateTime.UtcNow
        };

        context.Mascotas.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new MascotaDto
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Especie = entity.Especie,
            Raza = entity.Raza,
            Edad = entity.Edad,
            Peso = entity.Peso,
            Alergias = entity.Alergias,
            Foto = entity.FotoUrl
        };
    }
}
