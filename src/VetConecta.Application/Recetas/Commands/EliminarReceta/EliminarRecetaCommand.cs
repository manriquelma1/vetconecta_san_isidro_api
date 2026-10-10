using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Recetas.Commands.EliminarReceta;

public record EliminarRecetaCommand(Guid Id) : IRequest;

public class EliminarRecetaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<EliminarRecetaCommand>
{
    public async Task Handle(EliminarRecetaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.EsPersonal)
        {
            throw new UnauthorizedAccessException("Solo el personal veterinario puede eliminar recetas.");
        }

        var receta = await context.Recetas.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Receta), request.Id);

        context.Recetas.Remove(receta);
        await context.SaveChangesAsync(cancellationToken);
    }
}
