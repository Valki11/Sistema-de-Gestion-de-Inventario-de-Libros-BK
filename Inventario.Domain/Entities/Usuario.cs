namespace Inventario.Domain.Entities;

public class Usuario
{
    public decimal IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string ContrasenaUsuario { get; set; } = null!;
    public decimal IdRol { get; set; }

    public Rol Rol { get; set; } = null!;
    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
