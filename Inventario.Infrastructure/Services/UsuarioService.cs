using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Inventario.Domain.Entities;
using Inventario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

public class UsuarioService : IUsuarioService
{
    private readonly BibliotecaDbContext _db;
    public UsuarioService(BibliotecaDbContext db) => _db = db;

    public Task<bool> ExisteUsuarioAsync(decimal idUsuario, CancellationToken ct = default)
        => _db.Usuarios.AsNoTracking().AnyAsync(u => u.IdUsuario == idUsuario, ct);

public async Task<IEnumerable<UsuarioDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Select(u => new UsuarioDto(
                u.IdUsuario,
                u.NombreUsuario,
                u.Rol.NombreRol
            ))
            .ToListAsync(ct);
    }

    public async Task<UsuarioDto?> GetByIdAsync(decimal id, CancellationToken ct = default)
    {
        var u = await _db.Usuarios.AsNoTracking()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.IdUsuario == id, ct);

        return u is null ? null : new UsuarioDto(u.IdUsuario, u.NombreUsuario, u.Rol.NombreRol);
    }

    public async Task<decimal> CreateAsync(UsuarioCreateDto dto, CancellationToken ct = default)
    {
        var entity = new Usuario
        {
            NombreUsuario = dto.NombreUsuario,
            ContrasenaUsuario = dto.ContrasenaUsuario,
            IdRol = dto.IdRol
        };

        _db.Usuarios.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity.IdUsuario;
    }

    public async Task<bool> UpdateAsync(decimal id, UsuarioUpdateDto dto, CancellationToken ct = default)
{
    var entity = await _db.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id, ct);
    if (entity is null) return false;

    entity.NombreUsuario = dto.NombreUsuario;
    entity.IdRol = dto.IdRol;

    if (!string.IsNullOrWhiteSpace(dto.ContrasenaUsuario))
        entity.ContrasenaUsuario = dto.ContrasenaUsuario;

    await _db.SaveChangesAsync(ct);
    return true;
}


    public async Task<bool> DeleteAsync(decimal id, CancellationToken ct = default)
    {
        var entity = await _db.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id, ct);
        if (entity is null) return false;

        _db.Usuarios.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
