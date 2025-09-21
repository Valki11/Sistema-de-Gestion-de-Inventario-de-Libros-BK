using System;

namespace Inventario.Domain.Entities;

public class Libro
{
    public decimal IdLibro { get; set; }
    public string TituloLibro { get; set; } = null!;
    public int? AnioDePublicacion { get; set; }
    public string? GeneroLibro { get; set; }
    public decimal? NumeroCopias { get; set; }
    public decimal IdAutor { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public string? EstadoLibro { get; set; }

    public Autor Autor { get; set; } = null!;
    public ICollection<Prestamo> Prestamos { get; set; } = new System.Collections.Generic.List<Prestamo>();
}
