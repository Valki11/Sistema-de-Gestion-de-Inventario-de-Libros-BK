using System.Collections.Generic;

namespace Inventario.Domain.Entities;

public class Rol
{
    public decimal IdRol { get; set; }
    public string NombreRol { get; set; } = null!;

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
