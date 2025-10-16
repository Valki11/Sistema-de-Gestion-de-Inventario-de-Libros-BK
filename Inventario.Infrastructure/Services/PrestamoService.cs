using Inventario.Application.Contracts;
using Inventario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

public class PrestamoService : IPrestamoService
{
    private readonly BibliotecaDbContext _db;
    public PrestamoService(BibliotecaDbContext db) => _db = db;

    public async Task<decimal> CrearPrestamoAsync(decimal idLibro, decimal idUsuario, CancellationToken ct = default)
    {
        var libro = await _db.Libros.FirstOrDefaultAsync(l => l.IdLibro == idLibro, ct)
            ?? throw new InvalidOperationException("Libro no encontrado");

        if ((libro.NumeroCopias ?? 0) <= 0)
            throw new InvalidOperationException("No hay copias disponibles");

        if (libro.EstadoLibro == "NO DISPONIBLE")
                    throw new InvalidOperationException("No se encuentra disponible");

        var usuario = await _db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario, ct)
            ?? throw new InvalidOperationException("Usuario no encontrado");

        if (!string.Equals(usuario.Rol.NombreRol, "LECTOR", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("El préstamo solo puede asignarse a usuarios con rol LECTOR");

        libro.NumeroCopias = (libro.NumeroCopias ?? 0) - 1;

        var prestamo = new Prestamo
        {
            IdLibro = idLibro,
            IdUsuario = idUsuario,
            FechaPrestamo = DateTime.Now,
            Estado = "NO DISPONIBLE"
        };

        _db.Prestamos.Add(prestamo);
        await _db.SaveChangesAsync(ct);

        return prestamo.IdPrestamo; 
    }

    public async Task<bool> DevolverPrestamoAsync(decimal idPrestamo, CancellationToken ct = default)
    {
        var prestamo = await _db.Prestamos
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.IdPrestamo == idPrestamo, ct);

        if (prestamo is null) return false;
        if (string.Equals(prestamo.Estado, "DISPONIBLE", StringComparison.OrdinalIgnoreCase)) return true;

        prestamo.Estado = "DISPONIBLE";
        prestamo.FechaDevolucion = DateTime.Now;
        prestamo.Libro.NumeroCopias = (prestamo.Libro.NumeroCopias ?? 0) + 1;

        await _db.SaveChangesAsync(ct);
        return true;
    }

}
