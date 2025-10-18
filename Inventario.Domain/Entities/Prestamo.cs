using System;

namespace Inventario.Domain.Entities;

public class Prestamo
{
    public decimal IdPrestamo { get; set; }
    public decimal IdLibro { get; set; }
    public decimal IdUsuario { get; set; }

    public DateTime FechaPrestamo { get; set; }  
    public DateTime? FechaDevolucion { get; set; }
    public DateTime FechaRegistroPrestamo { get; set; }
    public string Estado { get; set; } = null!;

    public Libro Libro { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
