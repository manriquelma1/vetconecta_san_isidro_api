using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(u => u.Correo).HasMaxLength(150).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(300).IsRequired();
        builder.Property(u => u.Rol).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(u => u.Proveedor).HasMaxLength(20).IsRequired();

        builder.HasIndex(u => u.Correo).IsUnique();
    }
}
