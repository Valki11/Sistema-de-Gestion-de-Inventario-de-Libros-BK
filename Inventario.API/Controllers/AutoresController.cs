using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly IAutorService _service;
    public AutoresController(IAutorService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:decimal}")]
    public async Task<ActionResult<AutorDto>> GetById(decimal id, CancellationToken ct)
    {
        var a = await _service.GetByIdAsync(id, ct);
        return a is null ? NotFound() : Ok(a);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] AutorCreateDto dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:decimal}")]
    public async Task<ActionResult> Update(decimal id, [FromBody] AutorUpdateDto dto, CancellationToken ct)
        => await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:decimal}")]
    public async Task<ActionResult> Delete(decimal id, CancellationToken ct)
        => await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
