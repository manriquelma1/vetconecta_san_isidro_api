using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Enums;

namespace VetConecta.Infrastructure.Identity;

/// <summary>Lee el usuario autenticado de los claims del JWT en HttpContext.</summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? Usuario => httpContextAccessor.HttpContext?.User;

    public Guid? UsuarioId
    {
        get
        {
            var valor = Usuario?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? Usuario?.FindFirstValue("sub");
            return Guid.TryParse(valor, out var id) ? id : null;
        }
    }

    public RolUsuario? Rol
    {
        get
        {
            var valor = Usuario?.FindFirstValue(ClaimTypes.Role);
            return Enum.TryParse<RolUsuario>(valor, out var rol) ? rol : null;
        }
    }

    public bool EsPersonal => Rol == RolUsuario.personal;
}
