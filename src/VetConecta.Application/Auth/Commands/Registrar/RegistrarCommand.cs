using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;
using VetConecta.Domain.Enums;

namespace VetConecta.Application.Auth.Commands.Registrar;

public record RegistrarCommand : IRequest<AuthResultDto>
{
    public string Nombre { get; init; } = default!;
    public string Correo { get; init; } = default!;
    public string Clave { get; init; } = default!;
}

public class RegistrarCommandValidator : AbstractValidator<RegistrarCommand>
{
    public RegistrarCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Correo).NotEmpty().EmailAddress().MaximumLength(150);
        // Misma regla minima que validaba el formulario de registro en Angular.
        RuleFor(x => x.Clave).NotEmpty().MinimumLength(6);
    }
}

public class RegistrarCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IRequestHandler<RegistrarCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(RegistrarCommand request, CancellationToken cancellationToken)
    {
        var correo = request.Correo.Trim().ToLowerInvariant();

        var existe = await context.Usuarios.AnyAsync(u => u.Correo == correo, cancellationToken);
        if (existe)
        {
            throw new ValidationException(
                [new FluentValidation.Results.ValidationFailure(nameof(request.Correo), "Ya existe una cuenta con ese correo.")]);
        }

        var usuario = new Usuario
        {
            Nombre = request.Nombre.Trim(),
            Correo = correo,
            Rol = RolUsuario.propietario, // el registro publico siempre crea propietarios
            Proveedor = "correo",
            CreatedAt = DateTime.UtcNow
        };
        usuario.PasswordHash = passwordHasher.Hash(request.Clave);

        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto
        {
            Token = jwtTokenService.GenerarToken(usuario),
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol
        };
    }
}
