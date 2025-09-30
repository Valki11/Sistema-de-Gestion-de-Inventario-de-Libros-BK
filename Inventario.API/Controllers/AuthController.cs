using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var result = await _auth.LoginAsync(request, ct);
        return result is null ? Unauthorized(new { message = "Credenciales inválidas" }) : Ok(result);
    }
}

