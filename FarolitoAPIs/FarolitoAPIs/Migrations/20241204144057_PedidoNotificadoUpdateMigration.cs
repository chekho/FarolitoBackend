using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class PedidoNotificadoUpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estatus",
                table: "pedido_notificado");

            migrationBuilder.AddColumn<bool>(
                name: "pedido_entregado",
                table: "pedido_notificado",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "pedido_enviado",
                table: "pedido_notificado",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pedido_entregado",
                table: "pedido_notificado");

            migrationBuilder.DropColumn(
                name: "pedido_enviado",
                table: "pedido_notificado");

            migrationBuilder.AddColumn<string>(
                name: "estatus",
                table: "pedido_notificado",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
