using Inventario.Application.DTOs;

namespace Inventario.Application.Contracts;

public interface IAutorService
{
    Task<IEnumerable<AutorDto>> GetAllAsync(CancellationToken ct = default);
    Task<AutorDto?> GetByIdAsync(decimal id, CancellationToken ct = default);
    Task<decimal> CreateAsync(AutorCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(decimal id, AutorUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(decimal id, CancellationToken ct = default);
}
