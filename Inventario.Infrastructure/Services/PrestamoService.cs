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
        
        var libro = await _db.Libros.FirstOrDefaultAsync(l => l.IdLibro == idLibro, ct);
        if (libro is null) throw new InvalidOperationException("Libro no encontrado.");
        if ((libro.NumeroCopias ?? 0) <= 0) throw new InvalidOperationException("No hay copias disponibles.");

        var prestamo = new Domain.Entities.Prestamo
        {
            IdLibro = idLibro,
            IdUsuario = idUsuario,
            FechaRegistroPrestamo = DateTime.UtcNow,
            FechaDevolucion = null
        };

        libro.NumeroCopias = (libro.NumeroCopias ?? 0) - 1;

        _db.Prestamos.Add(prestamo);
        await _db.SaveChangesAsync(ct);
        return prestamo.IdPrestamo;
    }

    public async Task<bool> DevolverPrestamoAsync(decimal idPrestamo, CancellationToken ct = default)
    {
        var p = await _db.Prestamos.FirstOrDefaultAsync(x => x.IdPrestamo == idPrestamo, ct);
        if (p is null) return false;

        if (p.FechaDevolucion is not null) return true; // ya devuelto

        p.FechaDevolucion = DateTime.UtcNow;

        var libro = await _db.Libros.FirstOrDefaultAsync(l => l.IdLibro == p.IdLibro, ct);
        if (libro is not null) libro.NumeroCopias = (libro.NumeroCopias ?? 0) + 1;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
