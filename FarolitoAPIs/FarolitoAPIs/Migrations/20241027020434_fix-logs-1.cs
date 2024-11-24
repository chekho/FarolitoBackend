using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class fixlogs1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_logs_LogsId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_LogsModulo_logs_LogsId",
                table: "LogsModulo");

            migrationBuilder.DropPrimaryKey(
                name: "PK__logs__3213E83F375D100A",
                table: "logs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LogsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LogsId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "logs",
                newName: "Logs");

            migrationBuilder.RenameColumn(
                name: "usuarioId",
                table: "Logs",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "moduloId",
                table: "Logs",
                newName: "ModuloId");

            migrationBuilder.RenameColumn(
                name: "fechahora",
                table: "Logs",
                newName: "FechaHora");

            migrationBuilder.RenameColumn(
                name: "cambio",
                table: "Logs",
                newName: "Cambio");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Logs",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

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
                name: "IX_LogsUsuario_UsuarioId",
                table: "LogsUsuario",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogsModulo_Logs_LogsId",
                table: "LogsModulo",
                column: "LogsId",
                principalTable: "Logs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsModulo_Logs_LogsId",
                table: "LogsModulo");

            migrationBuilder.DropTable(
                name: "LogsUsuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "logs");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "logs",
                newName: "usuarioId");

            migrationBuilder.RenameColumn(
                name: "ModuloId",
                table: "logs",
                newName: "moduloId");

            migrationBuilder.RenameColumn(
                name: "FechaHora",
                table: "logs",
                newName: "fechahora");

            migrationBuilder.RenameColumn(
                name: "Cambio",
                table: "logs",
                newName: "cambio");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "logs",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "LogsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__logs__3213E83F375D100A",
                table: "logs",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LogsId",
                table: "AspNetUsers",
                column: "LogsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_logs_LogsId",
                table: "AspNetUsers",
                column: "LogsId",
                principalTable: "logs",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogsModulo_logs_LogsId",
                table: "LogsModulo",
                column: "LogsId",
                principalTable: "logs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
