using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;

namespace VetConecta.Application.Recetas.Commands.CrearReceta;

public record MedicamentoInput
{
    public string Nombre { get; init; } = default!;
    public string Dosis { get; init; } = default!;
    public string Duracion { get; init; } = default!;
}

public record CrearRecetaCommand : IRequest<RecetaDto>
{
    public Guid MascotaId { get; init; }
    public string Veterinario { get; init; } = default!;
    public string FechaEmision { get; init; } = default!;
    public string FechaFin { get; init; } = default!;
    public List<MedicamentoInput> Medicamentos { get; init; } = [];
}

public class CrearRecetaCommandValidator : AbstractValidator<CrearRecetaCommand>
{
    public CrearRecetaCommandValidator()
    {
        RuleFor(x => x.MascotaId).NotEmpty();
        RuleFor(x => x.Veterinario).NotEmpty().MaximumLength(150);
        RuleFor(x => x.FechaEmision).NotEmpty().Must(f => DateOnly.TryParse(f, out _))
            .WithMessage("La fecha de emisión no es válida.");
        RuleFor(x => x.FechaFin).NotEmpty().Must(f => DateOnly.TryParse(f, out _))
            .WithMessage("La fecha de fin no es válida.");
        RuleFor(x => x).Must(x =>
            !DateOnly.TryParse(x.FechaEmision, out var emision) ||
            !DateOnly.TryParse(x.FechaFin, out var fin) ||
            fin >= emision)
            .WithMessage("El fin del tratamiento no puede ser anterior a la emisión.");
        RuleFor(x => x.Medicamentos).NotEmpty().WithMessage("Agrega al menos un medicamento.");
        RuleForEach(x => x.Medicamentos).ChildRules(medicamento =>
        {
            medicamento.RuleFor(m => m.Nombre).NotEmpty();
            medicamento.RuleFor(m => m.Dosis).NotEmpty();
            medicamento.RuleFor(m => m.Duracion).NotEmpty();
        });
    }
}

/// <summary>Solo el personal veterinario puede emitir recetas (igual que en Angular).</summary>
public class CrearRecetaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<CrearRecetaCommand, RecetaDto>
{
    public async Task<RecetaDto> Handle(CrearRecetaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede emitir recetas.");
        }

        var mascota = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == request.MascotaId, cancellationToken)
            ?? throw new ValidationException(
                [new FluentValidation.Results.ValidationFailure(nameof(request.MascotaId), "La mascota no existe.")]);

        var receta = new Receta
        {
            MascotaId = mascota.Id,
            Veterinario = request.Veterinario.Trim(),
            FechaEmision = DateOnly.Parse(request.FechaEmision),
            FechaFin = DateOnly.Parse(request.FechaFin),
            Medicamentos = request.Medicamentos.Select(m => new MedicamentoReceta
            {
                Nombre = m.Nombre.Trim(),
                Dosis = m.Dosis.Trim(),
                Duracion = m.Duracion.Trim()
            }).ToList()
        };

        context.Recetas.Add(receta);
        await context.SaveChangesAsync(cancellationToken);

        return new RecetaDto
        {
            Id = receta.Id,
            MascotaId = receta.MascotaId,
            Veterinario = receta.Veterinario,
            FechaEmision = receta.FechaEmision.ToString("yyyy-MM-dd"),
            FechaFin = receta.FechaFin.ToString("yyyy-MM-dd"),
            Medicamentos = receta.Medicamentos
                .Select(m => new MedicamentoDto { Id = m.Id, Nombre = m.Nombre, Dosis = m.Dosis, Duracion = m.Duracion })
                .ToList()
        };
    }
}
