-- Base de datos farolito para SQL Server

create database farolito_db;
use farolito_db;

-- -----------------------------------------------------
-- Schema 
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Table `detallesUsuario`
-- -----------------------------------------------------
CREATE TABLE detallesUsuario (
  id INT NOT NULL PRIMARY KEY,
  nombres VARCHAR(45) NULL,
  apellidoM VARCHAR(45) NULL,
  apellidoP VARCHAR(45) NULL,
  correo VARCHAR(45) NULL
);

-- -----------------------------------------------------
-- Table `usuario`
-- -----------------------------------------------------
CREATE TABLE usuario (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NULL,
  contraseña VARCHAR(255) NULL,
  token VARCHAR(45) NULL,
  rol VARCHAR(45) NULL,
  estatus TINYINT NULL,
  detallesUsuario_id INT NOT NULL,
  FOREIGN KEY (detallesUsuario_id) REFERENCES detallesUsuario(id)
);

-- -----------------------------------------------------
-- Table `compra`
-- -----------------------------------------------------
CREATE TABLE compra (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  fecha DATE NULL,
  usuario_id INT NOT NULL,
  FOREIGN KEY (usuario_id) REFERENCES [usuario](id)
);

-- -----------------------------------------------------
-- Table `detallecompra`
-- -----------------------------------------------------
CREATE TABLE detallecompra (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  lote VARCHAR(50) NULL,
  costo FLOAT NULL,
  compra_id INT NOT NULL,
  FOREIGN KEY (compra_id) REFERENCES compra(id)
);

-- -----------------------------------------------------
-- Table `receta`
-- -----------------------------------------------------
CREATE TABLE receta (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombrelampara VARCHAR(100) NULL,
  estatus INT NULL
);

-- -----------------------------------------------------
-- Table `solicitudproduccion`
-- -----------------------------------------------------
CREATE TABLE solicitudproduccion (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  Descripcion VARCHAR(255) NULL,
  estatus INT NULL,
  receta_id INT NOT NULL,
  usuario_id INT NOT NULL,
  FOREIGN KEY (receta_id) REFERENCES receta(id),
  FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

-- -----------------------------------------------------
-- Table `produccion`
-- -----------------------------------------------------
CREATE TABLE produccion (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  fecha DATE NULL,
  costo FLOAT NULL,
  usuario_id INT NOT NULL,
  solicitudproduccion_id INT NOT NULL,
  FOREIGN KEY (usuario_id) REFERENCES usuario(id),
  FOREIGN KEY (solicitudproduccion_id) REFERENCES solicitudproduccion(id)
);

-- -----------------------------------------------------
-- Table `proveedor`
-- -----------------------------------------------------
CREATE TABLE proveedor (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombreEmpresa VARCHAR(100) NULL,
  dirección VARCHAR(255) NULL,
  teléfono VARCHAR(20) NULL,
  nombreAtiende VARCHAR(100) NULL,
  apellidoM VARCHAR(45) NULL,
  apellidoP VARCHAR(45) NULL,
  estatus TINYINT NULL DEFAULT 1
);

-- -----------------------------------------------------
-- Table `componentes`
-- -----------------------------------------------------
CREATE TABLE componentes (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(150) NULL
);

-- -----------------------------------------------------
-- Table `inventariocomponentes`
-- -----------------------------------------------------
CREATE TABLE inventariocomponentes (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  proveedor_id INT NOT NULL,
  componentes_id INT NOT NULL,
  detallecompra_id INT NOT NULL,
  FOREIGN KEY (proveedor_id) REFERENCES proveedor(id),
  FOREIGN KEY (componentes_id) REFERENCES componentes(id),
  FOREIGN KEY (detallecompra_id) REFERENCES detallecompra(id)
);

-- -----------------------------------------------------
-- Table `detalleproduccion`
-- -----------------------------------------------------
CREATE TABLE detalleproduccion (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  produccion_id INT NOT NULL,
  inventariocomponentes_id INT NOT NULL,
  FOREIGN KEY (produccion_id) REFERENCES produccion(id),
  FOREIGN KEY (inventariocomponentes_id) REFERENCES inventariocomponentes(id)
);

-- -----------------------------------------------------
-- Table `venta`
-- -----------------------------------------------------
CREATE TABLE venta (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  fecha DATETIME NULL,
  descuento FLOAT NULL,
  folio VARCHAR(15) NULL,
  usuario_id INT NOT NULL,
  FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

-- -----------------------------------------------------
-- Table `inventariolampara`
-- -----------------------------------------------------
CREATE TABLE inventariolampara (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  precio FLOAT NULL,
  fechaCreacion DATE NULL,
  lote VARCHAR(15) NULL,
  receta_id INT NOT NULL,
  produccion_id INT NOT NULL,
  FOREIGN KEY (receta_id) REFERENCES receta(id),
  FOREIGN KEY (produccion_id) REFERENCES produccion(id)
);

-- -----------------------------------------------------
-- Table `detalleventa`
-- -----------------------------------------------------
CREATE TABLE detalleventa (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  precioUnitario FLOAT NULL,
  venta_id INT NOT NULL,
  inventariolampara_id INT NOT NULL,
  FOREIGN KEY (venta_id) REFERENCES venta(id),
  FOREIGN KEY (inventariolampara_id) REFERENCES inventariolampara(id)
);

-- -----------------------------------------------------
-- Table `componentesreceta`
-- -----------------------------------------------------
CREATE TABLE componentesreceta (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  estatus INT NULL,
  receta_id INT NOT NULL,
  componentes_id INT NOT NULL,
  FOREIGN KEY (receta_id) REFERENCES receta(id),
  FOREIGN KEY (componentes_id) REFERENCES componentes(id)
);

-- -----------------------------------------------------
-- Table `mermalampara`
-- -----------------------------------------------------
CREATE TABLE mermalampara (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  descripcion VARCHAR(700) NULL,
  fecha DATE NULL,
  usuario_id INT NOT NULL,
  inventariolampara_id INT NOT NULL,
  FOREIGN KEY (usuario_id) REFERENCES usuario(id),
  FOREIGN KEY (inventariolampara_id) REFERENCES inventariolampara(id)
);

-- -----------------------------------------------------
-- Table `mermacomponentes`
-- -----------------------------------------------------
CREATE TABLE mermacomponentes (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cantidad INT NULL,
  descripcion VARCHAR(700) NULL,
  fecha DATE NULL,
  usuario_id INT NOT NULL,
  inventariocomponentes_id INT NOT NULL,
  FOREIGN KEY (usuario_id) REFERENCES usuario(id),
  FOREIGN KEY (inventariocomponentes_id) REFERENCES inventariocomponentes(id)
);

-- -----------------------------------------------------
-- Table `productoproveedor`
-- -----------------------------------------------------
CREATE TABLE productoproveedor (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  proveedor_id INT NOT NULL,
  componentes_id INT NOT NULL,
  FOREIGN KEY (proveedor_id) REFERENCES proveedor(id),
  FOREIGN KEY (componentes_id) REFERENCES componentes(id)
);

-- -----------------------------------------------------
-- Table `carrito`
-- -----------------------------------------------------
CREATE TABLE carrito (
  id INT NOT NULL PRIMARY KEY,
  fecha DATE NULL,
  stastus TINYINT NULL,
  receta_id INT NOT NULL,
  usuario_id INT NOT NULL,
  FOREIGN KEY (receta_id) REFERENCES receta(id),
  FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

-- -----------------------------------------------------
-- Table `pedido`
-- -----------------------------------------------------
CREATE TABLE pedido (
  id INT NOT NULL PRIMARY KEY,
  usuario_id INT NOT NULL,
  usuario_detallesUsuario_id INT NOT NULL,
  fecha VARCHAR(45) NULL,
  estatus TINYINT NULL,
  FOREIGN KEY (usuario_id) REFERENCES usuario(id),
  FOREIGN KEY (usuario_detallesUsuario_id) REFERENCES detallesUsuario(id)
);

-- -----------------------------------------------------
-- Table `detallePedido`
-- -----------------------------------------------------
CREATE TABLE detallePedido (
  id INT NOT NULL PRIMARY KEY,
  receta_id INT NOT NULL,
  pedido_id INT NOT NULL,
  FOREIGN KEY (receta_id) REFERENCES receta(id),
  FOREIGN KEY (pedido_id) REFERENCES pedido(id)
);
