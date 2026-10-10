using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VetConecta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CitasRecetasHistorialInternamientoProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Servicio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PropietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citas_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Usuarios_PropietarioId",
                        column: x => x.PropietarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Internamientos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Jaula = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MotivoIngreso = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FechaIngreso = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraIngreso = table.Column<TimeOnly>(type: "time", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaAlta = table.Column<DateOnly>(type: "date", nullable: true),
                    Temperatura = table.Column<decimal>(type: "decimal(4,1)", nullable: false),
                    FrecuenciaCardiaca = table.Column<int>(type: "int", nullable: false),
                    FrecuenciaRespiratoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Internamientos_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Veterinario = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FechaEmision = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recetas_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vacunas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FechaAplicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    ProximaDosis = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacunas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacunas_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitasMedicas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MascotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tratamiento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Veterinario = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitasMedicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitasMedicas_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Evoluciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternamientoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Veterinario = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evoluciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evoluciones_Internamientos_InternamientoId",
                        column: x => x.InternamientoId,
                        principalTable: "Internamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tratamientos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternamientoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Horario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Realizado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tratamientos_Internamientos_InternamientoId",
                        column: x => x.InternamientoId,
                        principalTable: "Internamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentosReceta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecetaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Dosis = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Duracion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentosReceta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentosReceta_Recetas_RecetaId",
                        column: x => x.RecetaId,
                        principalTable: "Recetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Internamientos",
                columns: new[] { "Id", "Estado", "FechaAlta", "FechaIngreso", "FrecuenciaCardiaca", "FrecuenciaRespiratoria", "HoraIngreso", "Jaula", "MascotaId", "MotivoIngreso", "Temperatura" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999901"), "Internado", null, new DateOnly(2026, 9, 20), 80, 20, new TimeOnly(8, 30, 0), "Jaula 04", new Guid("33333333-3333-3333-3333-333333333333"), "Observación y tratamiento veterinario", 38.5m });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Categoria", "Nombre", "Precio", "Stock", "StockMinimo" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"), "Farmacia", "NexGard Spectra", 65.00m, 12, 5 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), "Pet Shop", "Shampoo medicado Oti-Soothe", 42.90m, 8, 3 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa03"), "Pet Shop", "Royal Canin Gastrointestinal", 89.90m, 3, 5 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa04"), "Farmacia", "Amoxicilina 500 mg", 35.50m, 10, 4 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa05"), "Farmacia", "Meloxicam suspensión", 28.00m, 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "Recetas",
                columns: new[] { "Id", "FechaEmision", "FechaFin", "MascotaId", "Veterinario" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888801"), new DateOnly(2026, 9, 18), new DateOnly(2026, 9, 25), new Guid("44444444-4444-4444-4444-444444444444"), "Dr. Carlos Mendoza" },
                    { new Guid("88888888-8888-8888-8888-888888888802"), new DateOnly(2026, 9, 10), new DateOnly(2026, 9, 13), new Guid("33333333-3333-3333-3333-333333333333"), "Dra. Andrea Ruiz" }
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Rol",
                value: "propietario");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Rol",
                value: "personal");

            migrationBuilder.InsertData(
                table: "Vacunas",
                columns: new[] { "Id", "FechaAplicacion", "MascotaId", "Nombre", "ProximaDosis" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666601"), new DateOnly(2026, 3, 12), new Guid("33333333-3333-3333-3333-333333333333"), "Séxtuple canina", new DateOnly(2027, 3, 12) },
                    { new Guid("66666666-6666-6666-6666-666666666602"), new DateOnly(2026, 4, 5), new Guid("33333333-3333-3333-3333-333333333333"), "Antirrábica", new DateOnly(2027, 4, 5) },
                    { new Guid("66666666-6666-6666-6666-666666666603"), new DateOnly(2025, 5, 10), new Guid("33333333-3333-3333-3333-333333333333"), "Bordetella", new DateOnly(2026, 5, 10) }
                });

            migrationBuilder.InsertData(
                table: "VisitasMedicas",
                columns: new[] { "Id", "Descripcion", "Diagnostico", "Fecha", "Hora", "MascotaId", "Motivo", "Tratamiento", "Veterinario" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777701"), "Paciente presenta tos seca desde hace 3 días. Se realiza evaluación clínica.", "Traqueobronquitis leve", new DateOnly(2026, 3, 14), new TimeOnly(10, 30, 0), new Guid("33333333-3333-3333-3333-333333333333"), "Revisión por tos crónica", "Broncodilatador y reposo por 5 días", "Dr. Alejandro Gómez" });

            migrationBuilder.InsertData(
                table: "Evoluciones",
                columns: new[] { "Id", "Fecha", "Hora", "InternamientoId", "Nota", "Veterinario" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999921"), new DateOnly(2026, 9, 20), new TimeOnly(12, 30, 0), new Guid("99999999-9999-9999-9999-999999999901"), "Paciente estable, tolera el tratamiento y permanece en observación.", "Dra. Andrea Ruiz" });

            migrationBuilder.InsertData(
                table: "MedicamentosReceta",
                columns: new[] { "Id", "Dosis", "Duracion", "Nombre", "RecetaId" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888811"), "1 tableta cada 12 horas", "7 días", "Amoxicilina 250mg", new Guid("88888888-8888-8888-8888-888888888801") },
                    { new Guid("88888888-8888-8888-8888-888888888812"), "0.5 ml cada 24 horas", "3 días", "Meloxicam suspensión", new Guid("88888888-8888-8888-8888-888888888801") },
                    { new Guid("88888888-8888-8888-8888-888888888813"), "0.5 ml cada 24 horas", "3 días", "Meloxicam 2mg", new Guid("88888888-8888-8888-8888-888888888802") }
                });

            migrationBuilder.InsertData(
                table: "Tratamientos",
                columns: new[] { "Id", "Detalle", "Horario", "InternamientoId", "Nombre", "Realizado" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999911"), "Dr. Ruiz", "08:00", new Guid("99999999-9999-9999-9999-999999999901"), "Cefalexina 500mg (IV)", true },
                    { new Guid("99999999-9999-9999-9999-999999999912"), "Pendiente", "12:00", new Guid("99999999-9999-9999-9999-999999999901"), "Meloxicam 2mg (SC)", false },
                    { new Guid("99999999-9999-9999-9999-999999999913"), "50ml/h", "Continua", new Guid("99999999-9999-9999-9999-999999999901"), "Fluidoterapia (RL)", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_Fecha_Hora",
                table: "Citas",
                columns: new[] { "Fecha", "Hora" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_MascotaId",
                table: "Citas",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PropietarioId",
                table: "Citas",
                column: "PropietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Evoluciones_InternamientoId",
                table: "Evoluciones",
                column: "InternamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Internamientos_MascotaId",
                table: "Internamientos",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentosReceta_RecetaId",
                table: "MedicamentosReceta",
                column: "RecetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_MascotaId",
                table: "Recetas",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamientos_InternamientoId",
                table: "Tratamientos",
                column: "InternamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacunas_MascotaId",
                table: "Vacunas",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitasMedicas_MascotaId",
                table: "VisitasMedicas",
                column: "MascotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Evoluciones");

            migrationBuilder.DropTable(
                name: "MedicamentosReceta");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Tratamientos");

            migrationBuilder.DropTable(
                name: "Vacunas");

            migrationBuilder.DropTable(
                name: "VisitasMedicas");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropTable(
                name: "Internamientos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Rol",
                value: "Propietario");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Rol",
                value: "Personal");
        }
    }
}
