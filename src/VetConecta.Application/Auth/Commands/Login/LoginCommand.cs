using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;

namespace VetConecta.Application.Auth.Commands.Login;

public record LoginCommand : IRequest<AuthResultDto>
{
    public string Correo { get; init; } = default!;
    public string Clave { get; init; } = default!;
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Correo).NotEmpty().EmailAddress();
        RuleFor(x => x.Clave).NotEmpty();
    }
}

public class LoginCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var correo = request.Correo.Trim().ToLowerInvariant();

        var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo, cancellationToken);

        // Mismo mensaje tanto si el correo no existe como si la clave es incorrecta:
        // no revelamos cual de los dos datos fallo (HU-09, criterio 3).
        if (usuario is null || !passwordHasher.Verify(usuario.PasswordHash, request.Clave))
        {
            throw new UnauthorizedAccessException("Correo o clave incorrectos.");
        }

        return new AuthResultDto
        {
            Token = jwtTokenService.GenerarToken(usuario),
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol
        };
    }
}
