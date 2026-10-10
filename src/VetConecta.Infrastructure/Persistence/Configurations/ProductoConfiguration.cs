using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.Property(p => p.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Categoria).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Precio).HasColumnType("decimal(8,2)");
    }
}
