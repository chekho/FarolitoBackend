using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class daashboardCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ExistenciaComponente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Componente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Existencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExistenciaComponente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExistenciaLampara",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoTerminado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Existencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExistenciaLampara", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LamparaCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDeVentas = table.Column<int>(type: "int", nullable: false),
                    TotalGastado = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LamparaCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MejorCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalGastado = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MejorCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDeVentas = table.Column<int>(type: "int", nullable: false),
                    TotalRecaudado = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentasPeriodo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Año = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDeCompras = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasPeriodo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentasProductoPeriodo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDeVentas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasProductoPeriodo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExistenciaComponente");

            migrationBuilder.DropTable(
                name: "ExistenciaLampara");

            migrationBuilder.DropTable(
                name: "LamparaCliente");

            migrationBuilder.DropTable(
                name: "MejorCliente");

            migrationBuilder.DropTable(
                name: "VentaProducto");

            migrationBuilder.DropTable(
                name: "VentasPeriodo");

            migrationBuilder.DropTable(
                name: "VentasProductoPeriodo");

            migrationBuilder.CreateTable(
                name: "ComponentesComprados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadComprada = table.Column<int>(type: "int", nullable: false),
                    CostoTotalComprado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NombreComponente = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    CantidadUsado = table.Column<int>(type: "int", nullable: false),
                    Componente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usos = table.Column<int>(type: "int", nullable: false)
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
                    CantidadVendida = table.Column<int>(type: "int", nullable: false),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Comprado = table.Column<int>(type: "int", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mermado = table.Column<int>(type: "int", nullable: false),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PorcentajeMermado = table.Column<float>(type: "real", nullable: false),
                    TotalMermado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    TotalVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ventas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasMes", x => x.Id);
                });
        }
    }
}
