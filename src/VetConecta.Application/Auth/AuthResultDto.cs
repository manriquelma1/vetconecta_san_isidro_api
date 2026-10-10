using VetConecta.Domain.Enums;

namespace VetConecta.Application.Auth;

public record AuthResultDto
{
    public string Token { get; init; } = default!;
    public string Nombre { get; init; } = default!;
    public string Correo { get; init; } = default!;
    public RolUsuario Rol { get; init; }
}
