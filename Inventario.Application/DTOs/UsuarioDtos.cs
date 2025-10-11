namespace Inventario.Application.DTOs;

public record UsuarioDto(
    decimal IdUsuario,
    string NombreUsuario,
    string NombreRol
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
