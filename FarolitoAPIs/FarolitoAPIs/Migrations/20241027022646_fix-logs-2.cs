using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class fixlogs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsModulo");

            migrationBuilder.DropTable(
                name: "LogsUsuario");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Logs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ModuloId",
                table: "Logs",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UsuarioId",
                table: "Logs",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_AspNetUsers_UsuarioId",
                table: "Logs",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Modulos_ModuloId",
                table: "Logs",
                column: "ModuloId",
                principalTable: "Modulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_AspNetUsers_UsuarioId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Modulos_ModuloId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_ModuloId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UsuarioId",
                table: "Logs");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Logs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                        name: "FK_LogsModulo_Logs_LogsId",
                        column: x => x.LogsId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogsModulo_Modulos_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsUsuario",
                columns: table => new
                {
                    LogsId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsUsuario", x => new { x.LogsId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_LogsUsuario_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogsUsuario_Logs_LogsId",
                        column: x => x.LogsId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsModulo_ModuloId",
                table: "LogsModulo",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsUsuario_UsuarioId",
                table: "LogsUsuario",
                column: "UsuarioId");
        }
    }
}
