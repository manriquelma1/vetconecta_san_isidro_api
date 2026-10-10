using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VetConecta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InicialVetConecta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mascotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Especie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Raza = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Alergias = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FotoUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PropietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mascotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mascotas_Usuarios_PropietarioId",
                        column: x => x.PropietarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Correo", "CreatedAt", "Nombre", "PasswordHash", "Proveedor", "Rol", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "julian.campos@gmail.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Julián Campos", "AQAAAAEAACcQAAAAENZZwMjPWV1cSAMJPr+/RTpQdNp/wD1WhJScmTsFqzxFyUdlBbA07Wi96NI/WOJHxg==", "correo", "Propietario", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "personal@vetconecta.pe", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Personal veterinario", "AQAAAAEAACcQAAAAELqE3K6LQdg+EXlVT1ZWKIrXvcg3mMzuTYhvo9smAiGo6M20fyoTbrPIA5ZP/naUVw==", "correo", "Personal", null }
                });

            migrationBuilder.InsertData(
                table: "Mascotas",
                columns: new[] { "Id", "Alergias", "CreatedAt", "Edad", "Especie", "FotoUrl", "Nombre", "Peso", "PropietarioId", "Raza", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Ninguna", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Perro", null, "Colita", 12.5m, new Guid("11111111-1111-1111-1111-111111111111"), "Mestizo", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Ninguna", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Perro", null, "Luna", 24.0m, new Guid("11111111-1111-1111-1111-111111111111"), "Labrador", null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Ninguna", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Gato", null, "Simba", 5.2m, new Guid("11111111-1111-1111-1111-111111111111"), "Mestizo", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_PropietarioId",
                table: "Mascotas",
                column: "PropietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mascotas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
