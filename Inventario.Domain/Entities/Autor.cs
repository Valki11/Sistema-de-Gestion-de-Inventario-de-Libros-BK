namespace Inventario.Domain.Entities;

public class Autor
{
    public decimal IdAutor { get; set; }
    public string NombreAutor { get; set; } = null!;

    public ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
