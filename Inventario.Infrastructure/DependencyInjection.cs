using Inventario.Application.Contracts;
using Inventario.Infrastructure.Data;
using Inventario.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("OracleDb")
                  ?? throw new InvalidOperationException("Falta ConnectionStrings:OracleDb");

        services.AddDbContext<BibliotecaDbContext>(opt =>
        {
            opt.UseOracle(conn);
            
        });

        services.AddScoped<ILibroService, LibroService>();
        services.AddScoped<IPrestamoService, PrestamoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAutorService, AutorService>();

        return services;
    }
}
