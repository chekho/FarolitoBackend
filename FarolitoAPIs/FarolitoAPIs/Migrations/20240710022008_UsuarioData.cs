using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FarolitoAPIs.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioData : Migration
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
                    id = table.Column<int>(type: "int", nullable: false),
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
                    id = table.Column<int>(type: "int", nullable: false),
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
                    id = table.Column<int>(type: "int", nullable: false),
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
                    id = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.InsertData(
                table: "detallesUsuario",
                columns: new[] { "id", "apellidoM", "apellidoP", "correo", "nombres" },
                values: new object[,]
                {
                    { 1, "López", "Guerrero", "alexa@mail.com", "Alexa" },
                    { 2, "Cabrera", "Salazar", "chekho@mail.com", "Sergio de Jesús" },
                    { 3, "Almeida", "Ramirez", "almeida@mail.com", "Jose Angel" },
                    { 4, "Alvizo", "Juarez", "juarez@mail.com", "Angel Eduardo" },
                    { 5, "Luna", "Bravo", "adriandario@mail.com", "Adrián Darío" },
                    { 6, "López", "Pérez", "Pjuancarlos@mail.com", "Juan Carlos" },
                    { 7, "Martínez", "González", "Gmaríafernanda@mail.com", "María Fernanda" },
                    { 8, "Hernández", "Rodríguez", "Rjoséluis@mail.com", "José Luis" },
                    { 9, "Torres", "Ramírez", "Ranasofía@mail.com", "Ana Sofía" },
                    { 10, "Díaz", "Sánchez", "Spedro@mail.com", "Pedro" },
                    { 11, "García", "Moreno", "Mgabriela@mail.com", "Gabriela" },
                    { 12, "Mendoza", "Vargas", "Vcarlos@mail.com", "Carlos" },
                    { 13, "Romero", "Castillo", "Csofía@mail.com", "Sofía" },
                    { 14, "Delgado", "Ortiz", "Oluisfernando@mail.com", "Luis Fernando" },
                    { 15, "Silva", "Jiménez", "Jvaleria@mail.com", "Valeria" },
                    { 16, "Cruz", "Herrera", "Hjavier@mail.com", "Javier" },
                    { 17, "Morales", "Aguilar", "Adaniela@mail.com", "Daniela" },
                    { 18, "Flores", "Guzmán", "Gmiguelángel@mail.com", "Miguel Ángel" },
                    { 19, "Ruiz", "Soto", "Snatalia@mail.com", "Natalia" },
                    { 20, "Castro", "Molina", "Mandrés@mail.com", "Andrés" },
                    { 21, "Gómez", "Núñez", "Npaola@mail.com", "Paola" },
                    { 22, "Domínguez", "Ríos", "Rsebastián@mail.com", "Sebastián" },
                    { 23, "Vázquez", "León", "Llorena@mail.com", "Lorena" },
                    { 24, "Peña", "Ibáñez", "Iricardo@mail.com", "Ricardo" },
                    { 25, "Márquez", "Rivera", "Rteresa@mail.com", "Teresa" },
                    { 26, "Solís", "Fernández", "Feduardo@mail.com", "Eduardo" },
                    { 27, "Orozco", "Salazar", "Sadriana@mail.com", "Adriana" },
                    { 28, "Rivas", "Campos", "Cfrancisco@mail.com", "Francisco" },
                    { 29, "Paredes", "Medina", "Mmarcela@mail.com", "Marcela" },
                    { 30, "Villalobos", "Figueroa", "Fsantiago@mail.com", "Santiago" },
                    { 31, "Ortega", "Carrillo", "Cisabel@mail.com", "Isabel" },
                    { 32, "Navarro", "Blanco", "Bdiego@mail.com", "Diego" },
                    { 33, "Meza", "Reyes", "Rfernanda@mail.com", "Fernanda" },
                    { 34, "Duarte", "Escobar", "Ealejandro@mail.com", "Alejandro" },
                    { 35, "Valenzuela", "Vega", "Vcecilia@mail.com", "Cecilia" },
                    { 36, "Lara", "Montes", "Mmatías@mail.com", "Matías" },
                    { 37, "Cardona", "Fuentes", "Fregina@mail.com", "Regina" },
                    { 38, "Muñoz", "Miranda", "Mleonardo@mail.com", "Leonardo" },
                    { 39, "Serrano", "Ponce", "Pmiriam@mail.com", "Miriam" },
                    { 40, "Robles", "Castro", "Ctomás@mail.com", "Tomás" },
                    { 41, "Ayala", "Villanueva", "Valejandra@mail.com", "Alejandra" },
                    { 42, "Moya", "Peña", "Pesteban@mail.com", "Esteban" },
                    { 43, "Méndez", "Arce", "Aandrea@mail.com", "Andrea" },
                    { 44, "Quintana", "Robles", "Rbenjamín@mail.com", "Benjamín" },
                    { 45, "Cabrera", "Paredes", "Psofía@mail.com", "Sofía" },
                    { 46, "Cárdenas", "Sandoval", "Sjoaquín@mail.com", "Joaquín" },
                    { 47, "Valverde", "Méndez", "Mclaudia@mail.com", "Claudia" },
                    { 48, "Medina", "Cruz", "Cmarcos@mail.com", "Marcos" },
                    { 49, "Ávila", "Valdez", "Vcarolina@mail.com", "Carolina" },
                    { 50, "Rico", "Chávez", "Cemiliano@mail.com", "Emiliano" },
                    { 51, "Zamora", "Aguirre", "Avanessa@mail.com", "Vanessa" },
                    { 52, "Carrasco", "Ochoa", "Oguillermo@mail.com", "Guillermo" },
                    { 53, "Camacho", "Estrada", "Everónica@mail.com", "Verónica" },
                    { 54, "Villanueva", "Valenzuela", "Vmauricio@mail.com", "Mauricio" },
                    { 55, "Espinoza", "Tapia", "Tjulieta@mail.com", "Julieta" },
                    { 56, "Ponce", "Campos", "Cpablo@mail.com", "Pablo" },
                    { 57, "Valdés", "Delgado", "Dsara@mail.com", "Sara" },
                    { 58, "Valdivia", "Ortiz", "Ohugo@mail.com", "Hugo" },
                    { 59, "Palma", "Hernández", "Hlaura@mail.com", "Laura" },
                    { 60, "Suárez", "Rivas", "Rraúl@mail.com", "Raúl" },
                    { 61, "Montes", "Cordero", "Cirene@mail.com", "Irene" },
                    { 62, "Montero", "Meza", "Malfredo@mail.com", "Alfredo" },
                    { 63, "Téllez", "Marín", "Mpatricia@mail.com", "Patricia" },
                    { 64, "Tovar", "Rocha", "Rhernán@mail.com", "Hernán" },
                    { 65, "Soria", "Domínguez", "Dflorencia@mail.com", "Florencia" },
                    { 66, "Ordóñez", "Sepúlveda", "Svicente@mail.com", "Vicente" },
                    { 67, "Castañeda", "Olivares", "Olourdes@mail.com", "Lourdes" },
                    { 68, "Lara", "Ríos", "Rfabio@mail.com", "Fabio" },
                    { 69, "Cuenca", "Morales", "Mclara@mail.com", "Clara" },
                    { 70, "Barrios", "Soto", "Sesteban@mail.com", "Esteban" },
                    { 71, "Ramos", "Cabrera", "Camanda@mail.com", "Amanda" },
                    { 72, "Ochoa", "Salinas", "Srodrigo@mail.com", "Rodrigo" },
                    { 73, "Duarte", "Ramírez", "Rteresa@mail.com", "Teresa" },
                    { 74, "Morales", "Ponce", "Phugo@mail.com", "Hugo" },
                    { 75, "López", "Ortiz", "Odaniela@mail.com", "Daniela" },
                    { 76, "Cortés", "Villalobos", "Vgonzalo@mail.com", "Gonzalo" },
                    { 77, "Zúñiga", "Delgado", "Dbeatriz@mail.com", "Beatriz" },
                    { 78, "Rivera", "Morales", "Mfelipe@mail.com", "Felipe" },
                    { 79, "Ramírez", "Vega", "Vliliana@mail.com", "Liliana" },
                    { 80, "Arredondo", "Navarro", "Nignacio@mail.com", "Ignacio" },
                    { 81, "Barrera", "Miranda", "Madrián@mail.com", "Adrián" },
                    { 82, "Montoya", "Reyes", "Rteresa@mail.com", "Teresa" },
                    { 83, "Vargas", "Vázquez", "Vmartín@mail.com", "Martín" },
                    { 84, "Guzmán", "Ibáñez", "Ielena@mail.com", "Elena" },
                    { 85, "Castillo", "Carrillo", "Clucas@mail.com", "Lucas" },
                    { 86, "Cabrera", "Fuentes", "Fdolores@mail.com", "Dolores" },
                    { 87, "Rodríguez", "Escobar", "Ericardo@mail.com", "Ricardo" },
                    { 88, "Fernández", "Herrera", "Hsusana@mail.com", "Susana" }
                });

            migrationBuilder.InsertData(
                table: "usuario",
                columns: new[] { "id", "contraseña", "detallesUsuario_id", "estatus", "nombre", "rol", "token" },
                values: new object[,]
                {
                    { 1, "12345678", 1, (byte)1, "alexa", "Administrador", null },
                    { 2, "12345678", 2, (byte)1, "ramirez", "Administrador", null },
                    { 3, "12345678", 3, (byte)1, "almeida", "Administrador", null },
                    { 4, "12345678", 4, (byte)1, "chekho", "Administrador", null },
                    { 5, "12345678", 5, (byte)1, "dario", "Administrador", null },
                    { 6, "12345678A", 6, (byte)1, "Pjuancarlos", "Cliente", null },
                    { 7, "12345678A", 7, (byte)1, "Gmaríafernanda", "Cliente", null },
                    { 8, "12345678A", 8, (byte)1, "Rjoséluis", "Almacén", null },
                    { 9, "12345678A", 9, (byte)1, "Ranasofía", "Almacén", null },
                    { 10, "12345678A", 10, (byte)1, "Spedro", "Producción", null },
                    { 11, "12345678A", 11, (byte)1, "Mgabriela", "Producción", null },
                    { 12, "12345678A", 12, (byte)1, "Vcarlos", "Cliente", null },
                    { 13, "12345678A", 13, (byte)1, "Csofía", "Producción", null },
                    { 14, "12345678A", 14, (byte)1, "Oluisfernando", "Almacén", null },
                    { 15, "12345678A", 15, (byte)1, "Jvaleria", "Producción", null },
                    { 16, "12345678A", 16, (byte)1, "Hjavier", "Producción", null },
                    { 17, "12345678A", 17, (byte)1, "Adaniela", "Almacén", null },
                    { 18, "12345678A", 18, (byte)1, "Gmiguelángel", "Producción", null },
                    { 19, "12345678A", 19, (byte)1, "Snatalia", "Almacén", null },
                    { 20, "12345678A", 20, (byte)1, "Mandrés", "Almacén", null },
                    { 21, "12345678A", 21, (byte)1, "Npaola", "Almacén", null },
                    { 22, "12345678A", 22, (byte)1, "Rsebastián", "Producción", null },
                    { 23, "12345678A", 23, (byte)1, "Llorena", "Cliente", null },
                    { 24, "12345678A", 24, (byte)1, "Iricardo", "Almacén", null },
                    { 25, "12345678A", 25, (byte)1, "Rteresa", "Cliente", null },
                    { 26, "12345678A", 26, (byte)1, "Feduardo", "Producción", null },
                    { 27, "12345678A", 27, (byte)1, "Sadriana", "Almacén", null },
                    { 28, "12345678A", 28, (byte)1, "Cfrancisco", "Producción", null },
                    { 29, "12345678A", 29, (byte)1, "Mmarcela", "Producción", null },
                    { 30, "12345678A", 30, (byte)1, "Fsantiago", "Almacén", null },
                    { 31, "12345678A", 31, (byte)1, "Cisabel", "Cliente", null },
                    { 32, "12345678A", 32, (byte)1, "Bdiego", "Almacén", null },
                    { 33, "12345678A", 33, (byte)1, "Rfernanda", "Producción", null },
                    { 34, "12345678A", 34, (byte)1, "Ealejandro", "Producción", null },
                    { 35, "12345678A", 35, (byte)1, "Vcecilia", "Producción", null },
                    { 36, "12345678A", 36, (byte)1, "Mmatías", "Almacén", null },
                    { 37, "12345678A", 37, (byte)1, "Fregina", "Producción", null },
                    { 38, "12345678A", 38, (byte)1, "Mleonardo", "Almacén", null },
                    { 39, "12345678A", 39, (byte)1, "Pmiriam", "Cliente", null },
                    { 40, "12345678A", 40, (byte)1, "Ctomás", "Cliente", null },
                    { 41, "12345678A", 41, (byte)1, "Valejandra", "Almacén", null },
                    { 42, "12345678A", 42, (byte)1, "Pesteban", "Producción", null },
                    { 43, "12345678A", 43, (byte)1, "Aandrea", "Cliente", null },
                    { 44, "12345678A", 44, (byte)1, "Rbenjamín", "Producción", null },
                    { 45, "12345678A", 45, (byte)1, "Psofía", "Almacén", null },
                    { 46, "12345678A", 46, (byte)1, "Sjoaquín", "Producción", null },
                    { 47, "12345678A", 47, (byte)1, "Mclaudia", "Producción", null },
                    { 48, "12345678A", 48, (byte)1, "Cmarcos", "Cliente", null },
                    { 49, "12345678A", 49, (byte)1, "Vcarolina", "Cliente", null },
                    { 50, "12345678A", 50, (byte)1, "Cemiliano", "Producción", null },
                    { 51, "12345678A", 51, (byte)1, "Avanessa", "Producción", null },
                    { 52, "12345678A", 52, (byte)1, "Oguillermo", "Cliente", null },
                    { 53, "12345678A", 53, (byte)1, "Everónica", "Cliente", null },
                    { 54, "12345678A", 54, (byte)1, "Vmauricio", "Cliente", null },
                    { 55, "12345678A", 55, (byte)1, "Tjulieta", "Cliente", null },
                    { 56, "12345678A", 56, (byte)1, "Cpablo", "Cliente", null },
                    { 57, "12345678A", 57, (byte)1, "Dsara", "Producción", null },
                    { 58, "12345678A", 58, (byte)1, "Ohugo", "Almacén", null },
                    { 59, "12345678A", 59, (byte)1, "Hlaura", "Cliente", null },
                    { 60, "12345678A", 60, (byte)1, "Rraúl", "Almacén", null },
                    { 61, "12345678A", 61, (byte)1, "Cirene", "Producción", null },
                    { 62, "12345678A", 62, (byte)1, "Malfredo", "Producción", null },
                    { 63, "12345678A", 63, (byte)1, "Mpatricia", "Producción", null },
                    { 64, "12345678A", 64, (byte)1, "Rhernán", "Almacén", null },
                    { 65, "12345678A", 65, (byte)1, "Dflorencia", "Producción", null },
                    { 66, "12345678A", 66, (byte)1, "Svicente", "Producción", null },
                    { 67, "12345678A", 67, (byte)1, "Olourdes", "Almacén", null },
                    { 68, "12345678A", 68, (byte)1, "Rfabio", "Almacén", null },
                    { 69, "12345678A", 69, (byte)1, "Mclara", "Cliente", null },
                    { 70, "12345678A", 70, (byte)1, "Sesteban", "Cliente", null },
                    { 71, "12345678A", 71, (byte)1, "Camanda", "Producción", null },
                    { 72, "12345678A", 72, (byte)1, "Srodrigo", "Cliente", null },
                    { 73, "12345678A", 73, (byte)1, "Rteresa", "Almacén", null },
                    { 74, "12345678A", 74, (byte)1, "Phugo", "Cliente", null },
                    { 75, "12345678A", 75, (byte)1, "Odaniela", "Almacén", null },
                    { 76, "12345678A", 76, (byte)1, "Vgonzalo", "Producción", null },
                    { 77, "12345678A", 77, (byte)1, "Dbeatriz", "Almacén", null },
                    { 78, "12345678A", 78, (byte)1, "Mfelipe", "Producción", null },
                    { 79, "12345678A", 79, (byte)1, "Vliliana", "Producción", null },
                    { 80, "12345678A", 80, (byte)1, "Nignacio", "Producción", null },
                    { 81, "12345678A", 81, (byte)1, "Madrián", "Almacén", null },
                    { 82, "12345678A", 82, (byte)1, "Rteresa", "Cliente", null },
                    { 83, "12345678A", 83, (byte)1, "Vmartín", "Cliente", null },
                    { 84, "12345678A", 84, (byte)1, "Ielena", "Cliente", null },
                    { 85, "12345678A", 85, (byte)1, "Clucas", "Cliente", null },
                    { 86, "12345678A", 86, (byte)1, "Fdolores", "Producción", null },
                    { 87, "12345678A", 87, (byte)1, "Ericardo", "Cliente", null },
                    { 88, "12345678A", 88, (byte)1, "Hsusana", "Logística", null }
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
