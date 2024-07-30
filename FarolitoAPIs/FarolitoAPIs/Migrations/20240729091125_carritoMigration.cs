using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class carritoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha",
                table: "carrito");

            migrationBuilder.DropColumn(
                name: "stastus",
                table: "carrito");

            migrationBuilder.AddColumn<string>(
                name: "urlImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "urlImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateOnly>(
                name: "fecha",
                table: "carrito",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "stastus",
                table: "carrito",
                type: "tinyint",
                nullable: true);
        }
    }
}
