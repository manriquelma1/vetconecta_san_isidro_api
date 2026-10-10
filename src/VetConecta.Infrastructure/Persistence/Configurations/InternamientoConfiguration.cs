using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class InternamientoConfiguration : IEntityTypeConfiguration<Internamiento>
{
    public void Configure(EntityTypeBuilder<Internamiento> builder)
    {
        builder.Property(i => i.Jaula).HasMaxLength(50).IsRequired();
        builder.Property(i => i.MotivoIngreso).HasMaxLength(300).IsRequired();
        builder.Property(i => i.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(i => i.Temperatura).HasColumnType("decimal(4,1)");

        builder.HasOne(i => i.Mascota)
            .WithMany()
            .HasForeignKey(i => i.MascotaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Tratamientos)
            .WithOne(t => t.Internamiento)
            .HasForeignKey(t => t.InternamientoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Evoluciones)
            .WithOne(e => e.Internamiento)
            .HasForeignKey(e => e.InternamientoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TratamientoConfiguration : IEntityTypeConfiguration<Tratamiento>
{
    public void Configure(EntityTypeBuilder<Tratamiento> builder)
    {
        builder.Property(t => t.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(t => t.Horario).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Detalle).HasMaxLength(200).IsRequired();
    }
}

public class EvolucionInternamientoConfiguration : IEntityTypeConfiguration<EvolucionInternamiento>
{
    public void Configure(EntityTypeBuilder<EvolucionInternamiento> builder)
    {
        builder.Property(e => e.Nota).HasMaxLength(1000).IsRequired();
        builder.Property(e => e.Veterinario).HasMaxLength(150).IsRequired();
    }
}
