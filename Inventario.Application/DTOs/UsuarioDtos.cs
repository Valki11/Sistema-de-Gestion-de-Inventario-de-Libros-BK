namespace Inventario.Application.DTOs;

public record UsuarioDto(
    decimal IdUsuario,
    string NombreUsuario,
    string Rol
);

public record UsuarioCreateDto(
    string NombreUsuario,
    string ContrasenaUsuario,
    decimal IdRol
);

public record UsuarioUpdateDto(
    string NombreUsuario,
    string ContrasenaUsuario,
    decimal IdRol
);
