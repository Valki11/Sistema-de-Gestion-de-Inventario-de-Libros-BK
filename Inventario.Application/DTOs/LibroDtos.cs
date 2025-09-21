namespace Inventario.Application.DTOs;

public record LibroDto(
    decimal IdLibro,
    string TituloLibro,
    string Autor,
    int? AnioDePublicacion,
    string? GeneroLibro,
    decimal? NumeroCopias,
    string? EstadoLibro
);

public record LibroCreateDto(
    string TituloLibro,
    decimal IdAutor,
    int? AnioDePublicacion,
    string? GeneroLibro,
    decimal? NumeroCopias,
    string? EstadoLibro
);

public record LibroUpdateDto(
    string TituloLibro,
    decimal IdAutor,
    int? AnioDePublicacion,
    string? GeneroLibro,
    decimal? NumeroCopias,
    string? EstadoLibro
);
