namespace Inventario.Application.DTOs;

public record LoginRequestDto(string NombreUsuario, string ContrasenaUsuario);

public record LoginResponseDto(
    decimal IdUsuario,
    string NombreUsuario,
    string Rol
);
