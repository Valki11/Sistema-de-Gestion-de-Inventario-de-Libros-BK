using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Data;

public class BibliotecaDbContext : DbContext
{
    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options) { }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("BIBLIOTECA_USER");

        modelBuilder.ApplyConfiguration(new RolConfig());
        modelBuilder.ApplyConfiguration(new UsuarioConfig());
        modelBuilder.ApplyConfiguration(new AutorConfig());
        modelBuilder.ApplyConfiguration(new LibroConfig());
        modelBuilder.ApplyConfiguration(new PrestamoConfig());
    }
}

#region Configs

internal sealed class RolConfig : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> b)
    {
        b.ToTable("ROL");
        b.HasKey(x => x.IdRol);
        b.Property(x => x.IdRol).HasColumnName("ID_ROL");
        b.Property(x => x.NombreRol).HasColumnName("NOMBRE_ROL").HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.NombreRol).IsUnique();
    }
}

internal sealed class UsuarioConfig : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("USUARIO", "BIBLIOTECA_USER");  
        b.HasKey(x => x.IdUsuario);
        b.Property(x => x.IdUsuario).HasColumnName("ID_USUARIO");
        b.Property(x => x.NombreUsuario).HasColumnName("NOMBRE_USUARIO").HasMaxLength(100).IsRequired();
        b.Property(x => x.ContrasenaUsuario).HasColumnName("CONTRASENA_USUARIO").HasMaxLength(200).IsRequired();
        b.Property(x => x.IdRol).HasColumnName("ID_ROL").IsRequired();

        b.HasOne(x => x.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(x => x.IdRol)
            .HasConstraintName("FK_USUARIO_ROL");
    }
}


internal sealed class AutorConfig : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> b)
    {
        b.ToTable("AUTOR");
        b.HasKey(x => x.IdAutor);
        b.Property(x => x.IdAutor).HasColumnName("ID_AUTOR");
        b.Property(x => x.NombreAutor).HasColumnName("NOMBRE_AUTOR").HasMaxLength(150).IsRequired();
    }
}

internal sealed class LibroConfig : IEntityTypeConfiguration<Libro>
{
    public void Configure(EntityTypeBuilder<Libro> b)
    {
        b.ToTable("LIBRO");
        b.HasKey(x => x.IdLibro);
        b.Property(x => x.IdLibro).HasColumnName("ID_LIBRO");
        b.Property(x => x.TituloLibro).HasColumnName("TITULO_LIBRO").HasMaxLength(200).IsRequired();
        b.Property(x => x.AnioDePublicacion).HasColumnName("ANIO_DE_PUBLICACION");
        b.Property(x => x.GeneroLibro).HasColumnName("GENERO_LIBRO").HasMaxLength(100);
        b.Property(x => x.NumeroCopias).HasColumnName("NUMERO_COPIAS");
        b.Property(x => x.IdAutor).HasColumnName("ID_AUTOR").IsRequired();
        b.Property(x => x.FechaRegistro).HasColumnName("FECHA_REGISTRO");
        b.Property(x => x.EstadoLibro).HasColumnName("ESTADO_LIBRO").HasMaxLength(20);

        b.HasOne(x => x.Autor)
            .WithMany(a => a.Libros)
            .HasForeignKey(x => x.IdAutor)
            .HasConstraintName("FK_LIBRO_AUTOR");
    }
}

internal sealed class PrestamoConfig : IEntityTypeConfiguration<Prestamo>
{
    public void Configure(EntityTypeBuilder<Prestamo> b)
    {
        b.ToTable("PRESTAMO");
        b.HasKey(x => x.IdPrestamo);
        b.Property(x => x.IdPrestamo).HasColumnName("ID_PRESTAMO");
        b.Property(x => x.FechaRegistroPrestamo).HasColumnName("FECHA_REGISTRO_PRESTAMO");
        b.Property(x => x.IdLibro).HasColumnName("ID_LIBRO").IsRequired();
        b.Property(x => x.IdUsuario).HasColumnName("ID_USUARIO").IsRequired();
        b.Property(x => x.FechaDevolucion).HasColumnName("FECHA_DEVOLUCION");

        b.HasOne(x => x.Libro)
            .WithMany(l => l.Prestamos)
            .HasForeignKey(x => x.IdLibro)
            .HasConstraintName("FK_PRESTAMO_LIBRO");

        b.HasOne(x => x.Usuario)
            .WithMany(u => u.Prestamos)
            .HasForeignKey(x => x.IdUsuario)
            .HasConstraintName("FK_PRESTAMO_USUARIO");
    }
}

#endregion
