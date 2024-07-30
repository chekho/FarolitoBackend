using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class PedidosFechasMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha",
                table: "pedido");

            migrationBuilder.AlterColumn<string>(
                name: "estatus",
                table: "pedido",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaEntrega",
                table: "pedido",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaEnvio",
                table: "pedido",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaPedido",
                table: "pedido",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaEntrega",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "FechaEnvio",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "FechaPedido",
                table: "pedido");

            migrationBuilder.AlterColumn<byte>(
                name: "estatus",
                table: "pedido",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fecha",
                table: "pedido",
                type: "varchar(45)",
                unicode: false,
                maxLength: 45,
                nullable: true);
        }
    }
}
