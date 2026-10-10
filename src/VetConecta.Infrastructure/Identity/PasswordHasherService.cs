using Microsoft.AspNetCore.Identity;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Identity;

/// <summary>
/// Envuelve PasswordHasher&lt;Usuario&gt; de Microsoft.AspNetCore.Identity.
/// Se usa solo esta clase (hash + verificacion), no el resto del sistema de Identity.
/// </summary>
public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public string Hash(string password)
    {
        // El PasswordHasher no lee el objeto Usuario, solo lo exige por firma generica.
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string hash, string password)
    {
        var resultado = _hasher.VerifyHashedPassword(null!, hash, password);
        return resultado is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
