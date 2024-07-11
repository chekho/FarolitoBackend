using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataComponentes_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "proveedor",
                keyColumn: "id",
                keyValue: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "proveedor",
                columns: new[] { "id", "apellidoM", "apellidoP", "dirección", "estatus", "nombreAtiende", "nombreEmpresa", "teléfono" },
                values: new object[,]
                {
                    { 1, "Perez", "Mariel", "Jose Maria Morelos, 110", (byte)1, "Julian", "ORGON", "12345678" },
                    { 2, "Pérez", "López", "Avenida Insurgentes Sur 3500, Coyoacán, Ciudad de México, CDMX", (byte)1, "Juan Carlos", "Steren", "52 55 5604 3578" },
                    { 3, "González", "Martínez", "Avenida Mariano Escobedo 151, Anáhuac I Secc, Miguel Hidalgo, Ciudad de México, CDMX", (byte)1, "María Fernanda", "Gonher Proveedores", "52 55 5580 6000" },
                    { 4, "Rodríguez", "Hernández", "Isabel la Católica 36, Centro Histórico, Ciudad de México, CDMX", (byte)1, "José Luis", "Casa de las Lámparas", "52 55 5512 1398" },
                    { 5, "Ramírez", "Torres", "Calzada de Tlalpan 2735, Xotepingo, Coyoacán, Ciudad de México, CDMX", (byte)1, "Ana Sofía", "Distribuidora Eléctrica Mexicana", "52 55 5601 2105" },
                    { 6, "Sánchez", "Díaz", "Calle Victoria 57, Centro, Ciudad de México, CDMX", (byte)1, "Pedro", "Electrónica González", "52 55 5512 0594" },
                    { 7, "Moreno", "García", "Avenida Revolución 130, Tacubaya, Ciudad de México, CDMX", (byte)1, "Gabriela", "Lámparas y Más", "52 55 5272 3280" },
                    { 8, "Vargas", "Mendoza", "Av. del Taller 49, Transito, Cuauhtémoc, Ciudad de México, CDMX", (byte)1, "Carlos", "Conectores y Componentes", "52 55 5578 4001" }
                });
        }
    }
}
