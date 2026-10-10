using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetConecta.Application.Productos;
using VetConecta.Application.Productos.Commands.VenderProducto;
using VetConecta.Application.Productos.Queries.GetProductos;
using VetConecta.Domain.Enums;

namespace VetConecta.Api.Controllers;

/// <summary>Farmacia / Pet Shop (HU-10): solo personal.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(RolUsuario.personal))]
public class ProductosController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> GetProductos([FromQuery] string? texto)
        => Ok(await sender.Send(new GetProductosQuery(texto)));

    [HttpPost("{id:guid}/vender")]
    public async Task<ActionResult<ProductoDto>> Vender(Guid id, [FromBody] int cantidad = 1)
        => Ok(await sender.Send(new VenderProductoCommand { Id = id, Cantidad = cantidad }));
}
