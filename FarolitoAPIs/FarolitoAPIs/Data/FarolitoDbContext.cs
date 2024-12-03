using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FarolitoAPIs.Models;

namespace FarolitoAPIs.Data;

public partial class FarolitoDbContext : IdentityDbContext<Usuario>
{
    public FarolitoDbContext(DbContextOptions<FarolitoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Componente> Componentes { get; set; }

    public virtual DbSet<Componentesrecetum> Componentesreceta { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Detallecompra> Detallecompras { get; set; }

    public virtual DbSet<Detalleproduccion> Detalleproduccions { get; set; }

    public virtual DbSet<Detalleventum> Detalleventa { get; set; }

    public virtual DbSet<Inventariocomponente> Inventariocomponentes { get; set; }

    public virtual DbSet<Inventariolampara> Inventariolamparas { get; set; }

    public virtual DbSet<Logs> Logs { get; set; }

    public virtual DbSet<Mermacomponente> Mermacomponentes { get; set; }

    public virtual DbSet<Mermalampara> Mermalamparas { get; set; }

    public virtual DbSet<Modulo> Modulos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Produccion> Produccions { get; set; }

    public virtual DbSet<Productoproveedor> Productoproveedors { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Comentarios> Comentarioos { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<Solicitudproduccion> Solicitudproduccions { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    public virtual DbSet<HistorialComunicacion> HistorialComunicaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__carrito__3213E83F2DD1F388");

            entity.ToTable("carrito");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecetaId).HasColumnName("Receta_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Receta).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__carrito__receta___75A278F5");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__carrito__usuario__76969D2E");
        });

        modelBuilder.Entity<Componente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F2A28E243");

            entity.ToTable("componentes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Comentarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__comenta__3211E83F8B12441A");
            entity.ToTable("comentarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(700)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.UserId).HasColumnName("usuario_id");
        });

        modelBuilder.Entity<Componentesrecetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F6B05387A");

            entity.ToTable("componentesreceta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.ComponentesId).HasColumnName("componentes_id");
            entity.Property(e => e.Estatus).HasColumnName("estatus")
                .HasDefaultValue(null);
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");

            entity.HasOne(d => d.Componentes).WithMany(p => p.Componentesreceta)
                .HasForeignKey(d => d.ComponentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__component__compo__6754599E");

            entity.HasOne(d => d.Receta).WithMany(p => p.Componentesreceta)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__component__recet__66603565");
            entity.HasIndex(e => e.ComponentesId).HasDatabaseName("IX_Componentesreceta_ComponentesId");
            entity.HasIndex(e => e.RecetaId).HasDatabaseName("IX_Componentesreceta_RecetaId");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__compra__3213E83FF831483C");

            entity.ToTable("compra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Compras)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compra__usuario___3F466844");
        });

        modelBuilder.Entity<Detallecompra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__detallec__3213E83FD7591EEC");

            entity.ToTable("detallecompra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CompraId).HasColumnName("compra_id");
            entity.Property(e => e.Costo).HasColumnName("costo");
            entity.Property(e => e.Lote)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lote");

            entity.HasOne(d => d.Compra).WithMany(p => p.Detallecompras)
                .HasForeignKey(d => d.CompraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleco__compr__4222D4EF");
            entity.HasIndex(e => new { e.Costo, e.Cantidad }).HasDatabaseName("IX_Detallecompra_Costo_Cantidad");
        });

        modelBuilder.Entity<Detalleproduccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__detallep__3213E83FBE6428E3");

            entity.ToTable("detalleproduccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InventariocomponentesId).HasColumnName("inventariocomponentes_id");
            entity.Property(e => e.ProduccionId).HasColumnName("produccion_id");

            entity.HasOne(d => d.Inventariocomponentes).WithMany(p => p.Detalleproduccions)
                .HasForeignKey(d => d.InventariocomponentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detallepr__inven__59063A47");

            entity.HasOne(d => d.Produccion).WithMany(p => p.Detalleproduccions)
                .HasForeignKey(d => d.ProduccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detallepr__produ__5812160E");
        });

        modelBuilder.Entity<Detalleventum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__detallev__3213E83F3EC3A025");

            entity.ToTable("detalleventa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.InventariolamparaId).HasColumnName("inventariolampara_id");
            entity.Property(e => e.PrecioUnitario).HasColumnName("precioUnitario");
            entity.Property(e => e.VentaId).HasColumnName("venta_id");

            entity.HasOne(d => d.Inventariolampara).WithMany(p => p.Detalleventa)
                .HasForeignKey(d => d.InventariolamparaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleve__inven__6383C8BA");

            entity.HasOne(d => d.Venta).WithMany(p => p.Detalleventa)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleve__venta__628FA481");
        });

        modelBuilder.Entity<Inventariocomponente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventar__3213E83F20726ABE");

            entity.ToTable("inventariocomponentes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.ComponentesId).HasColumnName("componentes_id");
            entity.Property(e => e.DetallecompraId).HasColumnName("detallecompra_id");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");

            entity.HasOne(d => d.Componentes).WithMany(p => p.Inventariocomponentes)
                .HasForeignKey(d => d.ComponentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventari__compo__5441852A");

            entity.HasOne(d => d.Detallecompra).WithMany(p => p.Inventariocomponentes)
                .HasForeignKey(d => d.DetallecompraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventari__detal__5535A963");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Inventariocomponentes)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventari__prove__534D60F1");
        });

        modelBuilder.Entity<Inventariolampara>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventar__3213E83FB2AFE4ED");

            entity.ToTable("inventariolampara");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
            entity.Property(e => e.Lote)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("lote");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.ProduccionId).HasColumnName("produccion_id");
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");

            entity.HasOne(d => d.Produccion).WithMany(p => p.Inventariolamparas)
                .HasForeignKey(d => d.ProduccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventari__produ__5FB337D6");

            entity.HasOne(d => d.Receta).WithMany(p => p.Inventariolamparas)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventari__recet__5EBF139D");
        });

        modelBuilder.Entity<Mermacomponente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mermacom__3213E83F92DF7402");

            entity.ToTable("mermacomponentes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(700)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.InventariocomponentesId).HasColumnName("inventariocomponentes_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Inventariocomponentes).WithMany(p => p.Mermacomponentes)
                .HasForeignKey(d => d.InventariocomponentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mermacomp__inven__6EF57B66");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Mermacomponentes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mermacomp__usuar__6E01572D");
        });
        modelBuilder.Entity<Logs>()
            .HasOne(log => log.Usuario)
            .WithMany(usuario => usuario.Logs) // Asegúrate de que Usuario tenga ICollection<Logs>
            .HasForeignKey(log => log.UsuarioId);

        modelBuilder.Entity<Logs>()
            .HasOne(log => log.Modulo)
            .WithMany(modulo => modulo.Logs) // Asegúrate de que Modulo tenga ICollection<Logs>
            .HasForeignKey(log => log.ModuloId);
        /*
        modelBuilder.Entity<Logs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__logs__3213E83F375D100A");
            entity.ToTable("logs");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaHora).HasColumnName("fechahora");
            entity.Property(e => e.Cambio).HasColumnName("cambio");
            entity.Property(e => e.ModuloId).HasColumnName("moduloId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(log => log.Usuario)
                .WithMany(usuario => usuario.Logs) // Asegúrate de que Usuario tenga ICollection<Logs>
                .HasForeignKey(log => log.UsuarioId);

            entity.HasOne(log => log.Modulo)
                .WithMany(modulo => modulo.Logs) // Asegúrate de que Modulo tenga ICollection<Logs>
                .HasForeignKey(log => log.ModuloId);
        });*/

        modelBuilder.Entity<Mermalampara>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mermalam__3213E83F375D789B");

            entity.ToTable("mermalampara");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(700)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.InventariolamparaId).HasColumnName("inventariolampara_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Inventariolampara).WithMany(p => p.Mermalamparas)
                .HasForeignKey(d => d.InventariolamparaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mermalamp__inven__6B24EA82");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Mermalamparas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mermalamp__usuar__6A30C649");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedido__3213E83F40684E8F");

            entity.ToTable("pedido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.VentumId).HasColumnName("venta_id");

            entity.HasOne(d => d.Ventum)
                .WithOne(v => v.Pedido)
                .HasForeignKey<Pedido>(d => d.VentumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido__venta__12345678");
        });

        modelBuilder.Entity<Produccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producci__3213E83F59DE05A4");

            entity.ToTable("produccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Costo).HasColumnName("costo");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.SolicitudproduccionId).HasColumnName("solicitudproduccion_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Solicitudproduccion).WithMany(p => p.Produccions)
                .HasForeignKey(d => d.SolicitudproduccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__produccio__solic__4BAC3F29");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Produccions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__produccio__usuar__4AB81AF0");
        });

        modelBuilder.Entity<Productoproveedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producto__3213E83F233E46C0");

            entity.ToTable("productoproveedor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ComponentesId).HasColumnName("componentes_id");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");

            entity.HasOne(d => d.Componentes).WithMany(p => p.Productoproveedors)
                .HasForeignKey(d => d.ComponentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__productop__compo__72C60C4A");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productoproveedors)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__productop__prove__71D1E811");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__proveedo__3213E83F3984A0AA");

            entity.ToTable("proveedor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidoM");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidoP");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("dirección");
            entity.Property(e => e.Estatus)
                .HasDefaultValue(null)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreAtiende)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreAtiende");
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreEmpresa");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("teléfono");
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__receta__3213E83F99B7B827");

            entity.ToTable("receta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estatus)
                .HasDefaultValue(null)
                .HasColumnName("estatus");
            entity.Property(e => e.Nombrelampara)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombrelampara");
            entity.HasIndex(e => e.Id).HasDatabaseName("IX_Receta_Id ");
            entity.HasIndex(e => e.Estatus).HasDatabaseName("IX_Receta_Estatus");
            entity.HasIndex(e => e.Nombrelampara).HasDatabaseName("IX_Receta_Nombrelampara ");
        });

        modelBuilder.Entity<Solicitudproduccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__solicitu__3213E83FC8353465");

            entity.ToTable("solicitudproduccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Receta).WithMany(p => p.Solicitudproduccions)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__solicitud__recet__46E78A0C");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Solicitudproduccions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__solicitud__usuar__47DBAE45");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuario__3213E83F9E559931");

            entity.ToTable("AspNetUsers");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__venta__3213E83F96EBBD9A");

            entity.ToTable("venta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Folio)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("folio");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Venta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__venta__usuario_i__5BE2A6F2");
        });

        modelBuilder.Entity<HistorialComunicacion>(entity =>
        {
            entity.ToTable("historial_comunicacion");
            
            entity.HasKey(e => e.Id).HasName("PK_HistorialComunicacion");

            entity.Property(e => e.Id).HasColumnName("id");
            
            entity.Property(e => e.AccionRealizada)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("accion_realizada");

            entity.Property(e => e.Fecha)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("fecha");

            entity.Property(e => e.UsuarioId)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnName("usuario_id");
            
            entity.HasOne(e => e.usuario)
                .WithMany(u => u.HistorialComunicaciones)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HistorialComunicacion__Usuario");
        });
        
    var historialComunicacionData = new List<HistorialComunicacion>
    {
        new() { Id = 1, UsuarioId = "28", AccionRealizada = "Carrito abandonado", Fecha = new DateTime(2024, 05, 11) },
        new() { Id = 2, UsuarioId = "29", AccionRealizada = "Carrito abandonado", Fecha = new DateTime(2024, 05, 12) },
        new() { Id = 3, UsuarioId = "30", AccionRealizada = "Carrito abandonado", Fecha = new DateTime(2024, 05, 13) },
        new() { Id = 4, UsuarioId = "31", AccionRealizada = "Carrito abandonado", Fecha = new DateTime(2024, 05, 14) },
        new() { Id = 5, UsuarioId = "33", AccionRealizada = "Estado de compra modificado", Fecha = new DateTime(2024, 05, 15) },
        new() { Id = 6, UsuarioId = "34", AccionRealizada = "Estado de compra modificado", Fecha = new DateTime(2024, 05, 16) },
        new() { Id = 7, UsuarioId = "35", AccionRealizada = "Compra finalizada", Fecha = new DateTime(2024, 05, 17) },
        new() { Id = 8, UsuarioId = "36", AccionRealizada = "Compra finalizada", Fecha = new DateTime(2024, 05, 18) },
        new() { Id = 9, UsuarioId = "37", AccionRealizada = "Nueva compra", Fecha = new DateTime(2024, 05, 19) },
        new() { Id = 10, UsuarioId = "6", AccionRealizada = "Recuperación de contraseña", Fecha = new DateTime(2024, 05, 20) },
        new() { Id = 11, UsuarioId = "8", AccionRealizada = "Carrito abandonado", Fecha = new DateTime(2024, 05, 21) },
        new() { Id = 12, UsuarioId = "9", AccionRealizada = "Estado de compra modificado", Fecha = new DateTime(2024, 05, 22) },
        new() { Id = 13, UsuarioId = "28", AccionRealizada = "Compra finalizada", Fecha = new DateTime(2024, 05, 23) },
        new() { Id = 14, UsuarioId = "29", AccionRealizada = "Compra finalizada", Fecha = new DateTime(2024, 05, 24) },
        new() { Id = 15, UsuarioId = "30", AccionRealizada = "Carrito abandonado", Fecha = new DateTime(2024, 05, 25) },
        new() { Id = 16, UsuarioId = "31", AccionRealizada = "Estado de compra modificado", Fecha = new DateTime(2024, 05, 26) },
        new() { Id = 17, UsuarioId = "33", AccionRealizada = "Nueva compra", Fecha = new DateTime(2024, 05, 27) },
        new() { Id = 18, UsuarioId = "34", AccionRealizada = "Recuperación de contraseña", Fecha = new DateTime(2024, 05, 28) },
        new() { Id = 19, UsuarioId = "35", AccionRealizada = "Estado de compra modificado", Fecha = new DateTime(2024, 05, 29) },
        new() { Id = 20, UsuarioId = "36", AccionRealizada = "Compra finalizada", Fecha = new DateTime(2024, 05, 30) },
    };

    modelBuilder.Entity<HistorialComunicacion>().HasData(historialComunicacionData);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}