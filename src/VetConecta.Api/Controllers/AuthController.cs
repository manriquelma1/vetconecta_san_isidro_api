using MediatR;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Auth;
using VetConecta.Application.Auth.Commands.Login;
using VetConecta.Application.Auth.Commands.Registrar;

namespace VetConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login(LoginCommand command)
        => Ok(await sender.Send(command));

    [HttpPost("registro")]
    public async Task<ActionResult<AuthResultDto>> Registrar(RegistrarCommand command)
        => Ok(await sender.Send(command));
}
