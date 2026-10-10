using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Application.Mascotas.Commands.DeleteMascota;

public record DeleteMascotaCommand(Guid Id) : IRequest;

public class DeleteMascotaCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<DeleteMascotaCommand>
{
    public async Task Handle(DeleteMascotaCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Mascota), request.Id);

        if (!currentUser.EsPersonal && entity.PropietarioId != currentUser.UsuarioId)
        {
            throw new ForbiddenAccessException("No puedes eliminar la mascota de otro propietario.");
        }

        context.Mascotas.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
