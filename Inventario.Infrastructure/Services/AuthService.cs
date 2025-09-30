using Inventario.Application.Contracts;
using Inventario.Application.DTOs;
using Inventario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly BibliotecaDbContext _db;
    public AuthService(BibliotecaDbContext db) => _db = db;

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        // 🔎 Log: mostrar valores en texto y en ASCII
        Console.WriteLine($"[REQ] Usuario='{request.NombreUsuario}' (len={request.NombreUsuario?.Length})");
        if (!string.IsNullOrEmpty(request.NombreUsuario))
            Console.WriteLine("[REQ] Usuario HEX=" + string.Join(" ", request.NombreUsuario.Select(c => ((int)c).ToString())));

        Console.WriteLine($"[REQ] Pass   ='{request.ContrasenaUsuario}' (len={request.ContrasenaUsuario?.Length})");
        if (!string.IsNullOrEmpty(request.ContrasenaUsuario))
            Console.WriteLine("[REQ] Pass HEX=" + string.Join(" ", request.ContrasenaUsuario.Select(c => ((int)c).ToString())));

        var user = await _db.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u =>
                u.NombreUsuario.ToLower().Trim() == request.NombreUsuario.ToLower().Trim(), ct);

        if (user == null)
        {
            Console.WriteLine(" Usuario no encontrado en la BD");
            return null;
        }


        Console.WriteLine($"[DB] Usuario='{user.NombreUsuario}' (len={user.NombreUsuario?.Length})");
        Console.WriteLine("[DB] Usuario HEX=" + string.Join(" ", user.NombreUsuario.Select(c => ((int)c).ToString())));

        Console.WriteLine($"[DB] Pass   ='{user.ContrasenaUsuario}' (len={user.ContrasenaUsuario?.Length})");
        Console.WriteLine("[DB] Pass HEX=" + string.Join(" ", user.ContrasenaUsuario.Select(c => ((int)c).ToString())));

        if (!string.Equals(
                user.ContrasenaUsuario?.Trim(),
                request.ContrasenaUsuario?.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(" Contraseña no coincide");
            return null;
        }

        Console.WriteLine("Login exitoso");
        return new LoginResponseDto(user.IdUsuario, user.NombreUsuario, user.Rol.NombreRol);
    }
}
