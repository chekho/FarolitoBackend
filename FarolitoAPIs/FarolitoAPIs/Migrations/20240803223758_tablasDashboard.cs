using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class tablasDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentesComprados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreComponente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadComprada = table.Column<int>(type: "int", nullable: false),
                    CostoTotalComprado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentesComprados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentesUsados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Componente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usos = table.Column<int>(type: "int", nullable: false),
                    CantidadUsado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentesUsados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprasMes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    TotalCompras = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasMes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprasProveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCompras = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasProveedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasVendidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadVendida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasVendidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MermaProveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mermado = table.Column<int>(type: "int", nullable: false),
                    Comprado = table.Column<int>(type: "int", nullable: false),
                    TotalMermado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeMermado = table.Column<float>(type: "real", nullable: false),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MermaProveedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentasMes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Ventas = table.Column<int>(type: "int", nullable: false),
                    TotalVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasMes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentesComprados");

            migrationBuilder.DropTable(
                name: "ComponentesUsados");

            migrationBuilder.DropTable(
                name: "ComprasMes");

            migrationBuilder.DropTable(
                name: "ComprasProveedor");

            migrationBuilder.DropTable(
                name: "MasVendidos");

            migrationBuilder.DropTable(
                name: "MermaProveedor");

            migrationBuilder.DropTable(
                name: "VentasMes");
        }
    }
}
