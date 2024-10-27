using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class logscontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fechahora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cambio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    moduloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__logs__3213E83F375D100A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LogsModulo",
                columns: table => new
                {
                    LogsId = table.Column<int>(type: "int", nullable: false),
                    ModuloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsModulo", x => new { x.LogsId, x.ModuloId });
                    table.ForeignKey(
                        name: "FK_LogsModulo_Modulos_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogsModulo_logs_LogsId",
                        column: x => x.LogsId,
                        principalTable: "logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LogsId",
                table: "AspNetUsers",
                column: "LogsId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsModulo_ModuloId",
                table: "LogsModulo",
                column: "ModuloId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_logs_LogsId",
                table: "AspNetUsers",
                column: "LogsId",
                principalTable: "logs",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_logs_LogsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LogsModulo");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LogsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LogsId",
                table: "AspNetUsers");
        }
    }
}
