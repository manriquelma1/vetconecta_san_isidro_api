using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class MascotaConfiguration : IEntityTypeConfiguration<Mascota>
{
    public void Configure(EntityTypeBuilder<Mascota> builder)
    {
        builder.Property(m => m.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(m => m.Especie).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Raza).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Peso).HasColumnType("decimal(5,2)");
        builder.Property(m => m.Alergias).HasMaxLength(300);
        builder.Property(m => m.FotoUrl).HasMaxLength(300);

        builder.HasOne(m => m.Propietario)
            .WithMany(u => u.Mascotas)
            .HasForeignKey(m => m.PropietarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
