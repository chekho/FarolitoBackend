using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class RecetaCarrito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__carrito__receta___75A278F5",
                table: "carrito");

            migrationBuilder.RenameColumn(
                name: "Inventariolampara_id",
                table: "carrito",
                newName: "Receta_id");

            migrationBuilder.RenameIndex(
                name: "IX_carrito_Inventariolampara_id",
                table: "carrito",
                newName: "IX_carrito_Receta_id");

            migrationBuilder.AddForeignKey(
                name: "FK__carrito__receta___75A278F5",
                table: "carrito",
                column: "Receta_id",
                principalTable: "receta",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__carrito__receta___75A278F5",
                table: "carrito");

            migrationBuilder.RenameColumn(
                name: "Receta_id",
                table: "carrito",
                newName: "Inventariolampara_id");

            migrationBuilder.RenameIndex(
                name: "IX_carrito_Receta_id",
                table: "carrito",
                newName: "IX_carrito_Inventariolampara_id");

            migrationBuilder.AddForeignKey(
                name: "FK__carrito__receta___75A278F5",
                table: "carrito",
                column: "Inventariolampara_id",
                principalTable: "inventariolampara",
                principalColumn: "id");
        }
    }
}
