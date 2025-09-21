using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Inventario.Domain.Entities;
using Inventario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

public class AutorService : IAutorService
{
    private readonly BibliotecaDbContext _db;
    public AutorService(BibliotecaDbContext db) => _db = db;

    public async Task<IEnumerable<AutorDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Autores
            .AsNoTracking()
            .Select(a => new AutorDto(a.IdAutor, a.NombreAutor))
            .ToListAsync(ct);
    }

    public async Task<AutorDto?> GetByIdAsync(decimal id, CancellationToken ct = default)
    {
        var a = await _db.Autores.AsNoTracking().FirstOrDefaultAsync(a => a.IdAutor == id, ct);
        return a is null ? null : new AutorDto(a.IdAutor, a.NombreAutor);
    }

    public async Task<decimal> CreateAsync(AutorCreateDto dto, CancellationToken ct = default)
    {
        var entity = new Autor { NombreAutor = dto.NombreAutor };
        _db.Autores.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity.IdAutor;
    }

    public async Task<bool> UpdateAsync(decimal id, AutorUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _db.Autores.FirstOrDefaultAsync(x => x.IdAutor == id, ct);
        if (entity is null) return false;

        entity.NombreAutor = dto.NombreAutor;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(decimal id, CancellationToken ct = default)
    {
        var entity = await _db.Autores.FirstOrDefaultAsync(x => x.IdAutor == id, ct);
        if (entity is null) return false;

        _db.Autores.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
