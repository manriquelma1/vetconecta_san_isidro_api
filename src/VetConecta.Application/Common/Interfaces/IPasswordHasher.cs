namespace VetConecta.Application.Common.Interfaces;

/// <summary>
/// Abstrae el hashing de contrasenas: Application no debe depender de
/// Microsoft.AspNetCore.Identity directamente, eso vive en Infrastructure.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
