using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class HistorialComunicacionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "historial_comunicacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accion_realizada = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    usuario_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialComunicacion", x => x.id);
                    table.ForeignKey(
                        name: "FK__HistorialComunicacion__Usuario",
                        column: x => x.usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "historial_comunicacion",
                columns: new[] { "id", "accion_realizada", "fecha", "usuario_id" },
                values: new object[,]
                {
                    { 1, "Carrito abandonado", new DateTime(2024, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "28" },
                    { 2, "Carrito abandonado", new DateTime(2024, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "29" },
                    { 3, "Carrito abandonado", new DateTime(2024, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "30" },
                    { 4, "Carrito abandonado", new DateTime(2024, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "31" },
                    { 5, "Estado de compra modificado", new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "33" },
                    { 6, "Estado de compra modificado", new DateTime(2024, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "34" },
                    { 7, "Compra finalizada", new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "35" },
                    { 8, "Compra finalizada", new DateTime(2024, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "36" },
                    { 9, "Nueva compra", new DateTime(2024, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "37" },
                    { 10, "Recuperación de contraseña", new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "6" },
                    { 11, "Carrito abandonado", new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "8" },
                    { 12, "Estado de compra modificado", new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "9" },
                    { 13, "Compra finalizada", new DateTime(2024, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "28" },
                    { 14, "Compra finalizada", new DateTime(2024, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "29" },
                    { 15, "Carrito abandonado", new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "30" },
                    { 16, "Estado de compra modificado", new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "31" },
                    { 17, "Nueva compra", new DateTime(2024, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "33" },
                    { 18, "Recuperación de contraseña", new DateTime(2024, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "34" },
                    { 19, "Estado de compra modificado", new DateTime(2024, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "35" },
                    { 20, "Compra finalizada", new DateTime(2024, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "36" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_historial_comunicacion_usuario_id",
                table: "historial_comunicacion",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historial_comunicacion");
        }
    }
}
