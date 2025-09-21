using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Inventario.Domain.Entities;
using Inventario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

public class LibroService : ILibroService
{
    private readonly BibliotecaDbContext _db;

    public LibroService(BibliotecaDbContext db) => _db = db;

    public async Task<IEnumerable<LibroDto>> GetAllAsync(string? q = null, CancellationToken ct = default)
    {
        var query = _db.Libros
            .AsNoTracking()
            .Include(l => l.Autor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.ToUpper()}%";
            query = query.Where(l =>
                EF.Functions.Like(l.TituloLibro.ToUpper(), like) ||
                EF.Functions.Like(l.Autor.NombreAutor.ToUpper(), like));
        }

        return await query
            .OrderBy(l => l.TituloLibro)
            .Select(l => new LibroDto(
                l.IdLibro,
                l.TituloLibro,
                l.Autor.NombreAutor,
                l.AnioDePublicacion,
                l.GeneroLibro,
                l.NumeroCopias,
                l.EstadoLibro
            ))
            .ToListAsync(ct);
    }

    public async Task<LibroDto?> GetByIdAsync(decimal id, CancellationToken ct = default)
    {
        var l = await _db.Libros.AsNoTracking().Include(x => x.Autor)
            .FirstOrDefaultAsync(x => x.IdLibro == id, ct);

        return l is null ? null :
            new LibroDto(l.IdLibro, l.TituloLibro, l.Autor.NombreAutor, l.AnioDePublicacion, l.GeneroLibro, l.NumeroCopias, l.EstadoLibro);
    }

    public async Task<decimal> CreateAsync(LibroCreateDto dto, CancellationToken ct = default)
    {
        var entity = new Libro
        {
            TituloLibro = dto.TituloLibro,
            IdAutor = dto.IdAutor,
            AnioDePublicacion = dto.AnioDePublicacion,
            GeneroLibro = dto.GeneroLibro,
            NumeroCopias = dto.NumeroCopias,
            EstadoLibro = dto.EstadoLibro,
            FechaRegistro = DateTime.UtcNow
        };

        _db.Libros.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity.IdLibro;
    }

    public async Task<bool> UpdateAsync(decimal id, LibroUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _db.Libros.FirstOrDefaultAsync(x => x.IdLibro == id, ct);
        if (entity is null) return false;

        entity.TituloLibro = dto.TituloLibro;
        entity.IdAutor = dto.IdAutor;
        entity.AnioDePublicacion = dto.AnioDePublicacion;
        entity.GeneroLibro = dto.GeneroLibro;
        entity.NumeroCopias = dto.NumeroCopias;
        entity.EstadoLibro = dto.EstadoLibro;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(decimal id, CancellationToken ct = default)
    {
        var entity = await _db.Libros.FirstOrDefaultAsync(x => x.IdLibro == id, ct);
        if (entity is null) return false;

        _db.Libros.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
