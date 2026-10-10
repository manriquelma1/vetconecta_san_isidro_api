namespace VetConecta.Domain.Exceptions;

/// <summary>Usuario autenticado pero sin permiso para la accion (ej. propietario editando mascota ajena).</summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("No tienes permiso para realizar esta acción.") { }

    public ForbiddenAccessException(string message) : base(message) { }
}
