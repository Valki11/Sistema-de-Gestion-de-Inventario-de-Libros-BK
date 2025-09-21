using Inventario.Application.DTOs;

namespace Inventario.Application.Contracts;

public interface ILibroService
{
    Task<IEnumerable<LibroDto>> GetAllAsync(string? q = null, CancellationToken ct = default);
    Task<LibroDto?> GetByIdAsync(decimal id, CancellationToken ct = default);
    Task<decimal> CreateAsync(LibroCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(decimal id, LibroUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(decimal id, CancellationToken ct = default);
}
