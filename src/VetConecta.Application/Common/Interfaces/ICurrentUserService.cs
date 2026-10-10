using VetConecta.Domain.Enums;

namespace VetConecta.Application.Common.Interfaces;

/// <summary>Datos del usuario autenticado, leidos del JWT de la peticion actual.</summary>
public interface ICurrentUserService
{
    Guid? UsuarioId { get; }
    RolUsuario? Rol { get; }
    bool EsPersonal { get; }
}
