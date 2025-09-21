using Inventario.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoService _service;
    public PrestamosController(IPrestamoService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult> Crear([FromQuery] decimal idLibro, [FromQuery] decimal idUsuario, CancellationToken ct)
    {
        var id = await _service.CrearPrestamoAsync(idLibro, idUsuario, ct);
        return CreatedAtAction(nameof(Obtener), new { idPrestamo = id }, new { idPrestamo = id });
    }

    [HttpGet("{idPrestamo:decimal}")]
    public ActionResult Obtener([FromRoute] decimal idPrestamo)
        => Ok(new { idPrestamo });

    [HttpPost("{idPrestamo:decimal}/devolver")]
    public async Task<ActionResult> Devolver([FromRoute] decimal idPrestamo, CancellationToken ct)
        => await _service.DevolverPrestamoAsync(idPrestamo, ct) ? NoContent() : NotFound();
}
