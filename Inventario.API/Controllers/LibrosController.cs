using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly ILibroService _service;
    public LibrosController(ILibroService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LibroDto>>> GetAll([FromQuery] string? q, CancellationToken ct)
        => Ok(await _service.GetAllAsync(q, ct));

    [HttpGet("{id:decimal}")]
    public async Task<ActionResult<LibroDto>> GetById([FromRoute] decimal id, CancellationToken ct)
    {
        var libro = await _service.GetByIdAsync(id, ct);
        return libro is null ? NotFound() : Ok(libro);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] LibroCreateDto dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:decimal}")]
    public async Task<ActionResult> Update([FromRoute] decimal id, [FromBody] LibroUpdateDto dto, CancellationToken ct)
        => await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:decimal}")]
    public async Task<ActionResult> Delete([FromRoute] decimal id, CancellationToken ct)
        => await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
