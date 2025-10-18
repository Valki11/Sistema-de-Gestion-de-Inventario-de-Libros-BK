namespace Inventario.Application.Contracts;

public interface IPrestamoService
{
    Task<decimal> CrearPrestamoAsync(decimal idLibro, decimal idUsuario, CancellationToken ct = default);
    Task<bool> DevolverPrestamoAsync(decimal idPrestamo, CancellationToken ct = default);
    Task<IEnumerable<object>> ObtenerTodosAsync(CancellationToken ct = default);
}
