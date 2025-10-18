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
        modelBuilder.Entity<Autor>(entity =>        {
            entity.ToTable("AUTOR");

            entity.HasKey(e => e.IdAutor);

            entity.Property(e => e.IdAutor)
                .HasColumnName("ID_AUTOR")
                .ValueGeneratedOnAdd(); 
            entity.Property(e => e.NombreAutor)
                .HasColumnName("NOMBRE_AUTOR")
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("USUARIO");
            entity.HasKey(e => e.IdUsuario);

            entity.Property(e => e.IdUsuario)
                .HasColumnName("ID_USUARIO")
                .ValueGeneratedOnAdd(); 
                
            entity.Property(e => e.NombreUsuario)
                .HasColumnName("NOMBRE_USUARIO")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.ContrasenaUsuario)
                .HasColumnName("CONTRASENA_USUARIO")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.IdRol)
                .HasColumnName("ID_ROL")
                .IsRequired();
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.ToTable("PRESTAMO");

            entity.HasKey(e => e.IdPrestamo);

            entity.Property(e => e.IdPrestamo)
                .HasColumnName("ID_PRESTAMO")
                .ValueGeneratedOnAdd(); // 👈 importante para que EF Core entienda que Oracle genera el ID

            entity.Property(e => e.FechaRegistroPrestamo).HasColumnName("FECHA_REGISTRO_PRESTAMO");
            entity.Property(e => e.IdLibro).HasColumnName("ID_LIBRO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.FechaDevolucion).HasColumnName("FECHA_DEVOLUCION");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
        });

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
        b.Property(x => x.IdLibro).HasColumnName("ID_LIBRO").IsRequired();
        b.Property(x => x.IdUsuario).HasColumnName("ID_USUARIO").IsRequired();

        b.Property(x => x.FechaPrestamo).HasColumnName("FECHA_PRESTAMO"); 
        b.Property(x => x.FechaDevolucion).HasColumnName("FECHA_DEVOLUCION");
        b.Property(x => x.FechaRegistroPrestamo).HasColumnName("FECHA_REGISTRO_PRESTAMO");

        b.Property(x => x.Estado)
            .HasColumnName("ESTADO")
            .HasMaxLength(20)
            .IsRequired();

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
