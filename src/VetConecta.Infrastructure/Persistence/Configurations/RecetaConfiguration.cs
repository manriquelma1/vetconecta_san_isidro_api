using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class RecetaConfiguration : IEntityTypeConfiguration<Receta>
{
    public void Configure(EntityTypeBuilder<Receta> builder)
    {
        builder.Property(r => r.Veterinario).HasMaxLength(150).IsRequired();

        builder.HasOne(r => r.Mascota)
            .WithMany()
            .HasForeignKey(r => r.MascotaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Medicamentos)
            .WithOne(m => m.Receta)
            .HasForeignKey(m => m.RecetaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MedicamentoRecetaConfiguration : IEntityTypeConfiguration<MedicamentoReceta>
{
    public void Configure(EntityTypeBuilder<MedicamentoReceta> builder)
    {
        builder.Property(m => m.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(m => m.Dosis).HasMaxLength(150).IsRequired();
        builder.Property(m => m.Duracion).HasMaxLength(100).IsRequired();
    }
}
