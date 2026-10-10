using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetConecta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFotoMascotasSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mascotas",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "FotoUrl",
                value: "fotos/colita.svg");

            migrationBuilder.UpdateData(
                table: "Mascotas",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "FotoUrl",
                value: "fotos/luna.svg");

            migrationBuilder.UpdateData(
                table: "Mascotas",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "FotoUrl",
                value: "fotos/simba.svg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mascotas",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "FotoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Mascotas",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "FotoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Mascotas",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "FotoUrl",
                value: null);
        }
    }
}
