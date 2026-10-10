using Microsoft.EntityFrameworkCore;
using VetConecta.Domain.Entities;

namespace VetConecta.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Mascota> Mascotas { get; }
    DbSet<Cita> Citas { get; }
    DbSet<Vacuna> Vacunas { get; }
    DbSet<VisitaMedica> VisitasMedicas { get; }
    DbSet<Internamiento> Internamientos { get; }
    DbSet<Tratamiento> Tratamientos { get; }
    DbSet<EvolucionInternamiento> Evoluciones { get; }
    DbSet<Receta> Recetas { get; }
    DbSet<MedicamentoReceta> MedicamentosReceta { get; }
    DbSet<Producto> Productos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
