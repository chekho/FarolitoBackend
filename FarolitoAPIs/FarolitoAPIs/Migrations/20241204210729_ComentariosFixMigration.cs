using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class ComentariosFixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "usuario_id",
                table: "comentarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "usuario_id",
                table: "comentarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
