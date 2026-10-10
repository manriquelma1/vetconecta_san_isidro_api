using Microsoft.EntityFrameworkCore;
using VetConecta.Application.Common.Interfaces;
using VetConecta.Domain.Entities;
using VetConecta.Domain.Enums;
using VetConecta.Infrastructure.Persistence.Seed;

namespace VetConecta.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Vacuna> Vacunas => Set<Vacuna>();
    public DbSet<VisitaMedica> VisitasMedicas => Set<VisitaMedica>();
    public DbSet<Internamiento> Internamientos => Set<Internamiento>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<EvolucionInternamiento> Evoluciones => Set<EvolucionInternamiento>();
    public DbSet<Receta> Recetas => Set<Receta>();
    public DbSet<MedicamentoReceta> MedicamentosReceta => Set<MedicamentoReceta>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // HasData exige valores fijos (no Guid.NewGuid()/DateTime.UtcNow en cada build),
        // por eso las constantes viven en SeedIds en vez de generarse aqui.
        builder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = SeedIds.PropietarioDemoId,
                Nombre = "Julián Campos",
                Correo = "julian.campos@gmail.com",
                // Contraseña real: Cliente123
                PasswordHash = "AQAAAAEAACcQAAAAENZZwMjPWV1cSAMJPr+/RTpQdNp/wD1WhJScmTsFqzxFyUdlBbA07Wi96NI/WOJHxg==",
                Rol = RolUsuario.propietario,
                Proveedor = "correo",
                CreatedAt = SeedIds.FechaSemilla
            },
            new Usuario
            {
                Id = SeedIds.PersonalDemoId,
                Nombre = "Personal veterinario",
                Correo = "personal@vetconecta.pe",
                // Contraseña real: Vet12345
                PasswordHash = "AQAAAAEAACcQAAAAELqE3K6LQdg+EXlVT1ZWKIrXvcg3mMzuTYhvo9smAiGo6M20fyoTbrPIA5ZP/naUVw==",
                Rol = RolUsuario.personal,
                Proveedor = "correo",
                CreatedAt = SeedIds.FechaSemilla
            }
        );

        builder.Entity<Mascota>().HasData(
            new Mascota
            {
                Id = SeedIds.MascotaColitaId,
                Nombre = "Colita", Especie = "Perro", Raza = "Mestizo",
                Edad = 4, Peso = 12.5m, Alergias = "Ninguna", FotoUrl = "fotos/colita.svg",
                PropietarioId = SeedIds.PropietarioDemoId,
                CreatedAt = SeedIds.FechaSemilla
            },
            new Mascota
            {
                Id = SeedIds.MascotaLunaId,
                Nombre = "Luna", Especie = "Perro", Raza = "Labrador",
                Edad = 3, Peso = 24.0m, Alergias = "Ninguna", FotoUrl = "fotos/luna.svg",
                PropietarioId = SeedIds.PropietarioDemoId,
                CreatedAt = SeedIds.FechaSemilla
            },
            new Mascota
            {
                Id = SeedIds.MascotaSimbaId,
                Nombre = "Simba", Especie = "Gato", Raza = "Mestizo",
                Edad = 2, Peso = 5.2m, Alergias = "Ninguna", FotoUrl = "fotos/simba.svg",
                PropietarioId = SeedIds.PropietarioDemoId,
                CreatedAt = SeedIds.FechaSemilla
            }
        );

        builder.Entity<Vacuna>().HasData(
            new Vacuna
            {
                Id = SeedIds.VacunaSextupleId, MascotaId = SeedIds.MascotaColitaId,
                Nombre = "Séxtuple canina",
                FechaAplicacion = new DateOnly(2026, 3, 12), ProximaDosis = new DateOnly(2027, 3, 12)
            },
            new Vacuna
            {
                Id = SeedIds.VacunaAntirrabicaId, MascotaId = SeedIds.MascotaColitaId,
                Nombre = "Antirrábica",
                FechaAplicacion = new DateOnly(2026, 4, 5), ProximaDosis = new DateOnly(2027, 4, 5)
            },
            new Vacuna
            {
                Id = SeedIds.VacunaBordetellaId, MascotaId = SeedIds.MascotaColitaId,
                Nombre = "Bordetella",
                FechaAplicacion = new DateOnly(2025, 5, 10), ProximaDosis = new DateOnly(2026, 5, 10)
            }
        );

        builder.Entity<VisitaMedica>().HasData(
            new VisitaMedica
            {
                Id = SeedIds.VisitaColitaId, MascotaId = SeedIds.MascotaColitaId,
                Fecha = new DateOnly(2026, 3, 14), Hora = new TimeOnly(10, 30),
                Motivo = "Revisión por tos crónica",
                Descripcion = "Paciente presenta tos seca desde hace 3 días. Se realiza evaluación clínica.",
                Diagnostico = "Traqueobronquitis leve",
                Tratamiento = "Broncodilatador y reposo por 5 días",
                Veterinario = "Dr. Alejandro Gómez"
            }
        );

        builder.Entity<Receta>().HasData(
            new Receta
            {
                Id = SeedIds.RecetaLunaId, MascotaId = SeedIds.MascotaLunaId,
                Veterinario = "Dr. Carlos Mendoza",
                FechaEmision = new DateOnly(2026, 9, 18), FechaFin = new DateOnly(2026, 9, 25)
            },
            new Receta
            {
                Id = SeedIds.RecetaColitaId, MascotaId = SeedIds.MascotaColitaId,
                Veterinario = "Dra. Andrea Ruiz",
                FechaEmision = new DateOnly(2026, 9, 10), FechaFin = new DateOnly(2026, 9, 13)
            }
        );

        builder.Entity<MedicamentoReceta>().HasData(
            new MedicamentoReceta
            {
                Id = SeedIds.MedAmoxicilinaId, RecetaId = SeedIds.RecetaLunaId,
                Nombre = "Amoxicilina 250mg", Dosis = "1 tableta cada 12 horas", Duracion = "7 días"
            },
            new MedicamentoReceta
            {
                Id = SeedIds.MedMeloxicamSuspId, RecetaId = SeedIds.RecetaLunaId,
                Nombre = "Meloxicam suspensión", Dosis = "0.5 ml cada 24 horas", Duracion = "3 días"
            },
            new MedicamentoReceta
            {
                Id = SeedIds.MedMeloxicam2mgId, RecetaId = SeedIds.RecetaColitaId,
                Nombre = "Meloxicam 2mg", Dosis = "0.5 ml cada 24 horas", Duracion = "3 días"
            }
        );

        builder.Entity<Internamiento>().HasData(
            new Internamiento
            {
                Id = SeedIds.InternamientoColitaId, MascotaId = SeedIds.MascotaColitaId,
                Jaula = "Jaula 04", MotivoIngreso = "Observación y tratamiento veterinario",
                FechaIngreso = new DateOnly(2026, 9, 20), HoraIngreso = new TimeOnly(8, 30),
                Estado = EstadoInternamiento.Internado, FechaAlta = null,
                Temperatura = 38.5m, FrecuenciaCardiaca = 80, FrecuenciaRespiratoria = 20
            }
        );

        builder.Entity<Tratamiento>().HasData(
            new Tratamiento
            {
                Id = SeedIds.TratamientoCefalexinaId, InternamientoId = SeedIds.InternamientoColitaId,
                Nombre = "Cefalexina 500mg (IV)", Horario = "08:00", Detalle = "Dr. Ruiz", Realizado = true
            },
            new Tratamiento
            {
                Id = SeedIds.TratamientoMeloxicamId, InternamientoId = SeedIds.InternamientoColitaId,
                Nombre = "Meloxicam 2mg (SC)", Horario = "12:00", Detalle = "Pendiente", Realizado = false
            },
            new Tratamiento
            {
                Id = SeedIds.TratamientoFluidoterapiaId, InternamientoId = SeedIds.InternamientoColitaId,
                Nombre = "Fluidoterapia (RL)", Horario = "Continua", Detalle = "50ml/h", Realizado = false
            }
        );

        builder.Entity<EvolucionInternamiento>().HasData(
            new EvolucionInternamiento
            {
                Id = SeedIds.EvolucionColitaId, InternamientoId = SeedIds.InternamientoColitaId,
                Fecha = new DateOnly(2026, 9, 20), Hora = new TimeOnly(12, 30),
                Nota = "Paciente estable, tolera el tratamiento y permanece en observación.",
                Veterinario = "Dra. Andrea Ruiz"
            }
        );

        builder.Entity<Producto>().HasData(
            new Producto
            {
                Id = SeedIds.ProductoNexgardId, Nombre = "NexGard Spectra", Categoria = "Farmacia",
                Precio = 65.00m, Stock = 12, StockMinimo = 5
            },
            new Producto
            {
                Id = SeedIds.ProductoShampooId, Nombre = "Shampoo medicado Oti-Soothe", Categoria = "Pet Shop",
                Precio = 42.90m, Stock = 8, StockMinimo = 3
            },
            new Producto
            {
                Id = SeedIds.ProductoRoyalCaninId, Nombre = "Royal Canin Gastrointestinal", Categoria = "Pet Shop",
                Precio = 89.90m, Stock = 3, StockMinimo = 5
            },
            new Producto
            {
                Id = SeedIds.ProductoAmoxicilinaId, Nombre = "Amoxicilina 500 mg", Categoria = "Farmacia",
                Precio = 35.50m, Stock = 10, StockMinimo = 4
            },
            new Producto
            {
                Id = SeedIds.ProductoMeloxicamId, Nombre = "Meloxicam suspensión", Categoria = "Farmacia",
                Precio = 28.00m, Stock = 2, StockMinimo = 3
            }
        );

        base.OnModelCreating(builder);
    }
}
