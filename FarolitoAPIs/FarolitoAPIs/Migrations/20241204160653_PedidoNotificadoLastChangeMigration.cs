using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class PedidoNotificadoLastChangeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoNotificado",
                table: "pedido_notificado");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "pedido_notificado");

            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "pedido_notificado",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "pedido_notificado",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoNotificado",
                table: "pedido_notificado",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoNotificado",
                table: "pedido_notificado");

            migrationBuilder.DropColumn(
                name: "id",
                table: "pedido_notificado");

            migrationBuilder.AddColumn<int>(
                    name: "PedidoId",
                    table: "pedido_notificado",
                    type: "int",
                    nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoNotificado",
                table: "pedido_notificado",
                column: "PedidoId");
        }
    }
}
