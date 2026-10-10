using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetConecta.Domain.Entities;

namespace VetConecta.Infrastructure.Persistence.Configurations;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        builder.Property(c => c.Servicio).HasMaxLength(100).IsRequired();
        builder.Property(c => c.MotivoConsulta).HasMaxLength(500).IsRequired();
        builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasOne(c => c.Mascota)
            .WithMany()
            .HasForeignKey(c => c.MascotaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Propietario)
            .WithMany()
            .HasForeignKey(c => c.PropietarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Consulta mas frecuente: disponibilidad de horarios para una fecha.
        builder.HasIndex(c => new { c.Fecha, c.Hora });
    }
}
