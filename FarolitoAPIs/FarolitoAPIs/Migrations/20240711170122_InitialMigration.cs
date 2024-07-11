using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "componentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__componen__3213E83F2A28E243", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "detallesUsuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombres = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    apellidoM = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    apellidoP = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    correo = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detalles__3213E83F4F2303A5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "proveedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreEmpresa = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    dirección = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    teléfono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    nombreAtiende = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    apellidoM = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    apellidoP = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    estatus = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__proveedo__3213E83F3984A0AA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombrelampara = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    estatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__receta__3213E83F99B7B827", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    contraseña = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    token = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    rol = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    estatus = table.Column<byte>(type: "tinyint", nullable: true),
                    detallesUsuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuario__3213E83F9E559931", x => x.id);
                    table.ForeignKey(
                        name: "FK__usuario__detalle__3C69FB99",
                        column: x => x.detallesUsuario_id,
                        principalTable: "detallesUsuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productoproveedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    proveedor_id = table.Column<int>(type: "int", nullable: false),
                    componentes_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producto__3213E83F233E46C0", x => x.id);
                    table.ForeignKey(
                        name: "FK__productop__compo__72C60C4A",
                        column: x => x.componentes_id,
                        principalTable: "componentes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__productop__prove__71D1E811",
                        column: x => x.proveedor_id,
                        principalTable: "proveedor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "componentesreceta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    estatus = table.Column<int>(type: "int", nullable: true),
                    receta_id = table.Column<int>(type: "int", nullable: false),
                    componentes_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__componen__3213E83F6B05387A", x => x.id);
                    table.ForeignKey(
                        name: "FK__component__compo__6754599E",
                        column: x => x.componentes_id,
                        principalTable: "componentes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__component__recet__66603565",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "carrito",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    stastus = table.Column<byte>(type: "tinyint", nullable: true),
                    receta_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__carrito__3213E83F2DD1F388", x => x.id);
                    table.ForeignKey(
                        name: "FK__carrito__receta___75A278F5",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__carrito__usuario__76969D2E",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "compra",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__compra__3213E83FF831483C", x => x.id);
                    table.ForeignKey(
                        name: "FK__compra__usuario___3F466844",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    usuario_detallesUsuario_id = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    estatus = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pedido__3213E83F40684E8F", x => x.id);
                    table.ForeignKey(
                        name: "FK__pedido__usuario___797309D9",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__pedido__usuario___7A672E12",
                        column: x => x.usuario_detallesUsuario_id,
                        principalTable: "detallesUsuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "solicitudproduccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    estatus = table.Column<int>(type: "int", nullable: true),
                    receta_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__solicitu__3213E83FC8353465", x => x.id);
                    table.ForeignKey(
                        name: "FK__solicitud__recet__46E78A0C",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__solicitud__usuar__47DBAE45",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: true),
                    descuento = table.Column<double>(type: "float", nullable: true),
                    folio = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__venta__3213E83F96EBBD9A", x => x.id);
                    table.ForeignKey(
                        name: "FK__venta__usuario_i__5BE2A6F2",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "detallecompra",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    lote = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    costo = table.Column<double>(type: "float", nullable: true),
                    compra_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detallec__3213E83FD7591EEC", x => x.id);
                    table.ForeignKey(
                        name: "FK__detalleco__compr__4222D4EF",
                        column: x => x.compra_id,
                        principalTable: "compra",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "detallePedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receta_id = table.Column<int>(type: "int", nullable: false),
                    pedido_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detalleP__3213E83F51E236BA", x => x.id);
                    table.ForeignKey(
                        name: "FK__detallePe__pedid__7E37BEF6",
                        column: x => x.pedido_id,
                        principalTable: "pedido",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__detallePe__recet__7D439ABD",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "produccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    costo = table.Column<double>(type: "float", nullable: true),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    solicitudproduccion_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producci__3213E83F59DE05A4", x => x.id);
                    table.ForeignKey(
                        name: "FK__produccio__solic__4BAC3F29",
                        column: x => x.solicitudproduccion_id,
                        principalTable: "solicitudproduccion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__produccio__usuar__4AB81AF0",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "inventariocomponentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    proveedor_id = table.Column<int>(type: "int", nullable: false),
                    componentes_id = table.Column<int>(type: "int", nullable: false),
                    detallecompra_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__inventar__3213E83F20726ABE", x => x.id);
                    table.ForeignKey(
                        name: "FK__inventari__compo__5441852A",
                        column: x => x.componentes_id,
                        principalTable: "componentes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__inventari__detal__5535A963",
                        column: x => x.detallecompra_id,
                        principalTable: "detallecompra",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__inventari__prove__534D60F1",
                        column: x => x.proveedor_id,
                        principalTable: "proveedor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "inventariolampara",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    precio = table.Column<double>(type: "float", nullable: true),
                    fechaCreacion = table.Column<DateOnly>(type: "date", nullable: true),
                    lote = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    receta_id = table.Column<int>(type: "int", nullable: false),
                    produccion_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__inventar__3213E83FB2AFE4ED", x => x.id);
                    table.ForeignKey(
                        name: "FK__inventari__produ__5FB337D6",
                        column: x => x.produccion_id,
                        principalTable: "produccion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__inventari__recet__5EBF139D",
                        column: x => x.receta_id,
                        principalTable: "receta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "detalleproduccion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produccion_id = table.Column<int>(type: "int", nullable: false),
                    inventariocomponentes_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detallep__3213E83FBE6428E3", x => x.id);
                    table.ForeignKey(
                        name: "FK__detallepr__inven__59063A47",
                        column: x => x.inventariocomponentes_id,
                        principalTable: "inventariocomponentes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__detallepr__produ__5812160E",
                        column: x => x.produccion_id,
                        principalTable: "produccion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mermacomponentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(700)", unicode: false, maxLength: 700, nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    inventariocomponentes_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mermacom__3213E83F92DF7402", x => x.id);
                    table.ForeignKey(
                        name: "FK__mermacomp__inven__6EF57B66",
                        column: x => x.inventariocomponentes_id,
                        principalTable: "inventariocomponentes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__mermacomp__usuar__6E01572D",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "detalleventa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    precioUnitario = table.Column<double>(type: "float", nullable: true),
                    venta_id = table.Column<int>(type: "int", nullable: false),
                    inventariolampara_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detallev__3213E83F3EC3A025", x => x.id);
                    table.ForeignKey(
                        name: "FK__detalleve__inven__6383C8BA",
                        column: x => x.inventariolampara_id,
                        principalTable: "inventariolampara",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__detalleve__venta__628FA481",
                        column: x => x.venta_id,
                        principalTable: "venta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mermalampara",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(700)", unicode: false, maxLength: 700, nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    inventariolampara_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mermalam__3213E83F375D789B", x => x.id);
                    table.ForeignKey(
                        name: "FK__mermalamp__inven__6B24EA82",
                        column: x => x.inventariolampara_id,
                        principalTable: "inventariolampara",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__mermalamp__usuar__6A30C649",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_carrito_receta_id",
                table: "carrito",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "IX_carrito_usuario_id",
                table: "carrito",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_componentesreceta_componentes_id",
                table: "componentesreceta",
                column: "componentes_id");

            migrationBuilder.CreateIndex(
                name: "IX_componentesreceta_receta_id",
                table: "componentesreceta",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "IX_compra_usuario_id",
                table: "compra",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_detallecompra_compra_id",
                table: "detallecompra",
                column: "compra_id");

            migrationBuilder.CreateIndex(
                name: "IX_detallePedido_pedido_id",
                table: "detallePedido",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "IX_detallePedido_receta_id",
                table: "detallePedido",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalleproduccion_inventariocomponentes_id",
                table: "detalleproduccion",
                column: "inventariocomponentes_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalleproduccion_produccion_id",
                table: "detalleproduccion",
                column: "produccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalleventa_inventariolampara_id",
                table: "detalleventa",
                column: "inventariolampara_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalleventa_venta_id",
                table: "detalleventa",
                column: "venta_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventariocomponentes_componentes_id",
                table: "inventariocomponentes",
                column: "componentes_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventariocomponentes_detallecompra_id",
                table: "inventariocomponentes",
                column: "detallecompra_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventariocomponentes_proveedor_id",
                table: "inventariocomponentes",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventariolampara_produccion_id",
                table: "inventariolampara",
                column: "produccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventariolampara_receta_id",
                table: "inventariolampara",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "IX_mermacomponentes_inventariocomponentes_id",
                table: "mermacomponentes",
                column: "inventariocomponentes_id");

            migrationBuilder.CreateIndex(
                name: "IX_mermacomponentes_usuario_id",
                table: "mermacomponentes",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_mermalampara_inventariolampara_id",
                table: "mermalampara",
                column: "inventariolampara_id");

            migrationBuilder.CreateIndex(
                name: "IX_mermalampara_usuario_id",
                table: "mermalampara",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_usuario_detallesUsuario_id",
                table: "pedido",
                column: "usuario_detallesUsuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_usuario_id",
                table: "pedido",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_produccion_solicitudproduccion_id",
                table: "produccion",
                column: "solicitudproduccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_produccion_usuario_id",
                table: "produccion",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_productoproveedor_componentes_id",
                table: "productoproveedor",
                column: "componentes_id");

            migrationBuilder.CreateIndex(
                name: "IX_productoproveedor_proveedor_id",
                table: "productoproveedor",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudproduccion_receta_id",
                table: "solicitudproduccion",
                column: "receta_id");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudproduccion_usuario_id",
                table: "solicitudproduccion",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_detallesUsuario_id",
                table: "usuario",
                column: "detallesUsuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_venta_usuario_id",
                table: "venta",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carrito");

            migrationBuilder.DropTable(
                name: "componentesreceta");

            migrationBuilder.DropTable(
                name: "detallePedido");

            migrationBuilder.DropTable(
                name: "detalleproduccion");

            migrationBuilder.DropTable(
                name: "detalleventa");

            migrationBuilder.DropTable(
                name: "mermacomponentes");

            migrationBuilder.DropTable(
                name: "mermalampara");

            migrationBuilder.DropTable(
                name: "productoproveedor");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "inventariocomponentes");

            migrationBuilder.DropTable(
                name: "inventariolampara");

            migrationBuilder.DropTable(
                name: "componentes");

            migrationBuilder.DropTable(
                name: "detallecompra");

            migrationBuilder.DropTable(
                name: "proveedor");

            migrationBuilder.DropTable(
                name: "produccion");

            migrationBuilder.DropTable(
                name: "compra");

            migrationBuilder.DropTable(
                name: "solicitudproduccion");

            migrationBuilder.DropTable(
                name: "receta");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "detallesUsuario");
        }
    }
}
