using Inventario.Application.DTOs;
namespace Inventario.Application.Contracts;

public interface IUsuarioService
{
 
    Task<bool> ExisteUsuarioAsync(decimal idUsuario, CancellationToken ct = default);
    Task<IEnumerable<UsuarioDto>> GetAllAsync(CancellationToken ct = default);
    Task<UsuarioDto?> GetByIdAsync(decimal id, CancellationToken ct = default);
    Task<decimal> CreateAsync(UsuarioCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(decimal id, UsuarioUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(decimal id, CancellationToken ct = default);


}

