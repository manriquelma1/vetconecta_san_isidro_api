namespace VetConecta.Domain.Enums;

/// <summary>
/// Mismos dos roles que maneja hoy el Angular (usuario.ts: RolUsuario = 'propietario' | 'personal').
/// Los miembros van en minusculas, contra la convencion de C#, a proposito:
/// el JSON serializa el nombre del miembro tal cual (ver Program.cs), y asi
/// coincide exacto con el string que Angular espera, sin convertidores extra.
/// </summary>
public enum RolUsuario
{
    propietario,
    personal
}
