namespace Inventario.Application.DTOs;

public record AutorDto(
    decimal IdAutor,
    string NombreAutor
);

public record AutorCreateDto(
    string NombreAutor
);

public record AutorUpdateDto(
    string NombreAutor
);
