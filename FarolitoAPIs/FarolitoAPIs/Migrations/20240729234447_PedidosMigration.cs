using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class PedidosMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__pedido__usuario___797309D9",
                table: "pedido");

            migrationBuilder.DropTable(
                name: "detallePedido");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "pedido",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_pedido_usuario_id",
                table: "pedido",
                newName: "IX_pedido_UsuarioId");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "pedido",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "venta_id",
                table: "pedido",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_pedido_venta_id",
                table: "pedido",
                column: "venta_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__pedido__venta__12345678",
                table: "pedido",
                column: "venta_id",
                principalTable: "venta",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_pedido_AspNetUsers_UsuarioId",
                table: "pedido",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__pedido__venta__12345678",
                table: "pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_pedido_AspNetUsers_UsuarioId",
                table: "pedido");

            migrationBuilder.DropIndex(
                name: "IX_pedido_venta_id",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "venta_id",
                table: "pedido");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "pedido",
                newName: "usuario_id");

            migrationBuilder.RenameIndex(
                name: "IX_pedido_UsuarioId",
                table: "pedido",
                newName: "IX_pedido_usuario_id");

            migrationBuilder.AlterColumn<string>(
                name: "usuario_id",
                table: "pedido",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "detallePedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pedido_id = table.Column<int>(type: "int", nullable: false),
                    receta_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detalleP__3213E83F51E236BA", x => x.id);
                    table.ForeignKey(
                        name: "FK__detallePe__pedid__7E37BEF6",
                        column: x => x.pedido_id,
                        principalTable: "pedido",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__detallePe__recet__7D439ABD",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_detallePedido_pedido_id",
                table: "detallePedido",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "IX_detallePedido_receta_id",
                table: "detallePedido",
                column: "receta_id");

            migrationBuilder.AddForeignKey(
                name: "FK__pedido__usuario___797309D9",
                table: "pedido",
                column: "usuario_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
