using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class indexrecetasMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_componentesreceta_receta_id",
                table: "componentesreceta",
                newName: "IX_Componentesreceta_RecetaId");

            migrationBuilder.RenameIndex(
                name: "IX_componentesreceta_componentes_id",
                table: "componentesreceta",
                newName: "IX_Componentesreceta_ComponentesId");

            migrationBuilder.CreateIndex(
                name: "IX_Receta_Estatus",
                table: "receta",
                column: "estatus");

            migrationBuilder.CreateIndex(
                name: "IX_Receta_Id ",
                table: "receta",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Receta_Nombrelampara ",
                table: "receta",
                column: "nombrelampara");

            migrationBuilder.CreateIndex(
                name: "IX_Detallecompra_Costo_Cantidad",
                table: "detallecompra",
                columns: new[] { "costo", "cantidad" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Receta_Estatus",
                table: "receta");

            migrationBuilder.DropIndex(
                name: "IX_Receta_Id ",
                table: "receta");

            migrationBuilder.DropIndex(
                name: "IX_Receta_Nombrelampara ",
                table: "receta");

            migrationBuilder.DropIndex(
                name: "IX_Detallecompra_Costo_Cantidad",
                table: "detallecompra");

            migrationBuilder.RenameIndex(
                name: "IX_Componentesreceta_RecetaId",
                table: "componentesreceta",
                newName: "IX_componentesreceta_receta_id");

            migrationBuilder.RenameIndex(
                name: "IX_Componentesreceta_ComponentesId",
                table: "componentesreceta",
                newName: "IX_componentesreceta_componentes_id");
        }
    }
}
