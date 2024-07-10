using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class BeginProveedorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "proveedor",
                columns: new[] { "id", "apellidoM", "apellidoP", "dirección", "estatus", "nombreAtiende", "nombreEmpresa", "teléfono" },
                values: new object[] { 1, "Perez", "Mariel", "Jose Maria Morelos, 110", (byte)1, "Julian", "ORGON", "12345678" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
