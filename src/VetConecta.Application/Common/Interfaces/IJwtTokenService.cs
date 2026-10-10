using VetConecta.Domain.Entities;

namespace VetConecta.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerarToken(Usuario usuario);
}
