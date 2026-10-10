using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class VacunaConfiguration : IEntityTypeConfiguration<Vacuna>
{
    public void Configure(EntityTypeBuilder<Vacuna> builder)
    {
        builder.Property(v => v.Nombre).HasMaxLength(150).IsRequired();

        builder.HasOne(v => v.Mascota)
            .WithMany()
            .HasForeignKey(v => v.MascotaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class VisitaMedicaConfiguration : IEntityTypeConfiguration<VisitaMedica>
{
    public void Configure(EntityTypeBuilder<VisitaMedica> builder)
    {
        builder.Property(v => v.Motivo).HasMaxLength(200).IsRequired();
        builder.Property(v => v.Descripcion).HasMaxLength(1000);
        builder.Property(v => v.Diagnostico).HasMaxLength(500).IsRequired();
        builder.Property(v => v.Tratamiento).HasMaxLength(500).IsRequired();
        builder.Property(v => v.Veterinario).HasMaxLength(150).IsRequired();

        builder.HasOne(v => v.Mascota)
            .WithMany()
            .HasForeignKey(v => v.MascotaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
