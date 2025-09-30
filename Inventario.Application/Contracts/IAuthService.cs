using Inventario.Application.DTOs;

namespace Inventario.Application.Contracts;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
}
