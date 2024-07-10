using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Models;

public partial class FarolitoDbContext : DbContext
{
    public FarolitoDbContext()
    {
    }

    public FarolitoDbContext(DbContextOptions<FarolitoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Componente> Componentes { get; set; }

    public virtual DbSet<Componentesrecetum> Componentesreceta { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Detallecompra> Detallecompras { get; set; }

    public virtual DbSet<Detalleproduccion> Detalleproduccions { get; set; }

    public virtual DbSet<DetallesUsuario> DetallesUsuarios { get; set; }

    public virtual DbSet<Detalleventum> Detalleventa { get; set; }

    public virtual DbSet<Inventariocomponente> Inventariocomponentes { get; set; }

    public virtual DbSet<Inventariolampara> Inventariolamparas { get; set; }

    public virtual DbSet<Mermacomponente> Mermacomponentes { get; set; }

    public virtual DbSet<Mermalampara> Mermalamparas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Produccion> Produccions { get; set; }

    public virtual DbSet<Productoproveedor> Productoproveedors { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<Solicitudproduccion> Solicitudproduccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__carrito__3213E83F2DD1F388");

            entity.ToTable("carrito");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");
            entity.Property(e => e.Stastus).HasColumnName("stastus");
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

        modelBuilder.Entity<Componentesrecetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F6B05387A");

            entity.ToTable("componentesreceta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.ComponentesId).HasColumnName("componentes_id");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");

            entity.HasOne(d => d.Componentes).WithMany(p => p.Componentesreceta)
                .HasForeignKey(d => d.ComponentesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__component__compo__6754599E");

            entity.HasOne(d => d.Receta).WithMany(p => p.Componentesreceta)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__component__recet__66603565");
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

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__detalleP__3213E83F51E236BA");

            entity.ToTable("detallePedido");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PedidoId).HasColumnName("pedido_id");
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detallePe__pedid__7E37BEF6");

            entity.HasOne(d => d.Receta).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.RecetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detallePe__recet__7D439ABD");
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

        modelBuilder.Entity<DetallesUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__detalles__3213E83F4F2303A5");

            entity.ToTable("detallesUsuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidoM");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidoP");
            entity.Property(e => e.Correo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Nombres)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombres");

            entity.HasData(
                new DetallesUsuario { Id = 1, Nombres = "Alexa", ApellidoP = "Guerrero", ApellidoM = "López", Correo = "alexa@mail.com" },
                new DetallesUsuario { Id = 2, Nombres = "Sergio de Jesús", ApellidoP = "Salazar", ApellidoM = "Cabrera", Correo = "chekho@mail.com" },
                new DetallesUsuario { Id = 3, Nombres = "Jose Angel", ApellidoP = "Ramirez", ApellidoM = "Almeida", Correo = "almeida@mail.com" },
                new DetallesUsuario { Id = 4, Nombres = "Angel Eduardo", ApellidoP = "Juarez", ApellidoM = "Alvizo", Correo = "juarez@mail.com" },
                new DetallesUsuario { Id = 5, Nombres = "Adrián Darío", ApellidoP = "Bravo", ApellidoM = "Luna", Correo = "adriandario@mail.com" },

                new DetallesUsuario { Id = 6, Nombres = "Juan Carlos", ApellidoP = "Pérez", ApellidoM = "López", Correo = "Pjuancarlos@mail.com" },
                new DetallesUsuario { Id = 7, Nombres = "María Fernanda", ApellidoP = "González", ApellidoM = "Martínez", Correo = "Gmaríafernanda@mail.com" },
                new DetallesUsuario { Id = 8, Nombres = "José Luis", ApellidoP = "Rodríguez", ApellidoM = "Hernández", Correo = "Rjoséluis@mail.com" },
                new DetallesUsuario { Id = 9, Nombres = "Ana Sofía", ApellidoP = "Ramírez", ApellidoM = "Torres", Correo = "Ranasofía@mail.com" },
                new DetallesUsuario { Id = 10, Nombres = "Pedro", ApellidoP = "Sánchez", ApellidoM = "Díaz", Correo = "Spedro@mail.com" },
                new DetallesUsuario { Id = 11, Nombres = "Gabriela", ApellidoP = "Moreno", ApellidoM = "García", Correo = "Mgabriela@mail.com" },
                new DetallesUsuario { Id = 12, Nombres = "Carlos", ApellidoP = "Vargas", ApellidoM = "Mendoza", Correo = "Vcarlos@mail.com" },
                new DetallesUsuario { Id = 13, Nombres = "Sofía", ApellidoP = "Castillo", ApellidoM = "Romero", Correo = "Csofía@mail.com" },
                new DetallesUsuario { Id = 14, Nombres = "Luis Fernando", ApellidoP = "Ortiz", ApellidoM = "Delgado", Correo = "Oluisfernando@mail.com" },
                new DetallesUsuario { Id = 15, Nombres = "Valeria", ApellidoP = "Jiménez", ApellidoM = "Silva", Correo = "Jvaleria@mail.com" },
                new DetallesUsuario { Id = 16, Nombres = "Javier", ApellidoP = "Herrera", ApellidoM = "Cruz", Correo = "Hjavier@mail.com" },
                new DetallesUsuario { Id = 17, Nombres = "Daniela", ApellidoP = "Aguilar", ApellidoM = "Morales", Correo = "Adaniela@mail.com" },
                new DetallesUsuario { Id = 18, Nombres = "Miguel Ángel", ApellidoP = "Guzmán", ApellidoM = "Flores", Correo = "Gmiguelángel@mail.com" },
                new DetallesUsuario { Id = 19, Nombres = "Natalia", ApellidoP = "Soto", ApellidoM = "Ruiz", Correo = "Snatalia@mail.com" },
                new DetallesUsuario { Id = 20, Nombres = "Andrés", ApellidoP = "Molina", ApellidoM = "Castro", Correo = "Mandrés@mail.com" },
                new DetallesUsuario { Id = 21, Nombres = "Paola", ApellidoP = "Núñez", ApellidoM = "Gómez", Correo = "Npaola@mail.com" },
                new DetallesUsuario { Id = 22, Nombres = "Sebastián", ApellidoP = "Ríos", ApellidoM = "Domínguez", Correo = "Rsebastián@mail.com" },
                new DetallesUsuario { Id = 23, Nombres = "Lorena", ApellidoP = "León", ApellidoM = "Vázquez", Correo = "Llorena@mail.com" },
                new DetallesUsuario { Id = 24, Nombres = "Ricardo", ApellidoP = "Ibáñez", ApellidoM = "Peña", Correo = "Iricardo@mail.com" },
                new DetallesUsuario { Id = 25, Nombres = "Teresa", ApellidoP = "Rivera", ApellidoM = "Márquez", Correo = "Rteresa@mail.com" },
                new DetallesUsuario { Id = 26, Nombres = "Eduardo", ApellidoP = "Fernández", ApellidoM = "Solís", Correo = "Feduardo@mail.com" },
                new DetallesUsuario { Id = 27, Nombres = "Adriana", ApellidoP = "Salazar", ApellidoM = "Orozco", Correo = "Sadriana@mail.com" },
                new DetallesUsuario { Id = 28, Nombres = "Francisco", ApellidoP = "Campos", ApellidoM = "Rivas", Correo = "Cfrancisco@mail.com" },
                new DetallesUsuario { Id = 29, Nombres = "Marcela", ApellidoP = "Medina", ApellidoM = "Paredes", Correo = "Mmarcela@mail.com" },
                new DetallesUsuario { Id = 30, Nombres = "Santiago", ApellidoP = "Figueroa", ApellidoM = "Villalobos", Correo = "Fsantiago@mail.com" },
                new DetallesUsuario { Id = 31, Nombres = "Isabel", ApellidoP = "Carrillo", ApellidoM = "Ortega", Correo = "Cisabel@mail.com" },
                new DetallesUsuario { Id = 32, Nombres = "Diego", ApellidoP = "Blanco", ApellidoM = "Navarro", Correo = "Bdiego@mail.com" },
                new DetallesUsuario { Id = 33, Nombres = "Fernanda", ApellidoP = "Reyes", ApellidoM = "Meza", Correo = "Rfernanda@mail.com" },
                new DetallesUsuario { Id = 34, Nombres = "Alejandro", ApellidoP = "Escobar", ApellidoM = "Duarte", Correo = "Ealejandro@mail.com" },
                new DetallesUsuario { Id = 35, Nombres = "Cecilia", ApellidoP = "Vega", ApellidoM = "Valenzuela", Correo = "Vcecilia@mail.com" },
                new DetallesUsuario { Id = 36, Nombres = "Matías", ApellidoP = "Montes", ApellidoM = "Lara", Correo = "Mmatías@mail.com" },
                new DetallesUsuario { Id = 37, Nombres = "Regina", ApellidoP = "Fuentes", ApellidoM = "Cardona", Correo = "Fregina@mail.com" },
                new DetallesUsuario { Id = 38, Nombres = "Leonardo", ApellidoP = "Miranda", ApellidoM = "Muñoz", Correo = "Mleonardo@mail.com" },
                new DetallesUsuario { Id = 39, Nombres = "Miriam", ApellidoP = "Ponce", ApellidoM = "Serrano", Correo = "Pmiriam@mail.com" },
                new DetallesUsuario { Id = 40, Nombres = "Tomás", ApellidoP = "Castro", ApellidoM = "Robles", Correo = "Ctomás@mail.com" },
                new DetallesUsuario { Id = 41, Nombres = "Alejandra", ApellidoP = "Villanueva", ApellidoM = "Ayala", Correo = "Valejandra@mail.com" },
                new DetallesUsuario { Id = 42, Nombres = "Esteban", ApellidoP = "Peña", ApellidoM = "Moya", Correo = "Pesteban@mail.com" },
                new DetallesUsuario { Id = 43, Nombres = "Andrea", ApellidoP = "Arce", ApellidoM = "Méndez", Correo = "Aandrea@mail.com" },
                new DetallesUsuario { Id = 44, Nombres = "Benjamín", ApellidoP = "Robles", ApellidoM = "Quintana", Correo = "Rbenjamín@mail.com" },
                new DetallesUsuario { Id = 45, Nombres = "Sofía", ApellidoP = "Paredes", ApellidoM = "Cabrera", Correo = "Psofía@mail.com" },
                new DetallesUsuario { Id = 46, Nombres = "Joaquín", ApellidoP = "Sandoval", ApellidoM = "Cárdenas", Correo = "Sjoaquín@mail.com" },
                new DetallesUsuario { Id = 47, Nombres = "Claudia", ApellidoP = "Méndez", ApellidoM = "Valverde", Correo = "Mclaudia@mail.com" },
                new DetallesUsuario { Id = 48, Nombres = "Marcos", ApellidoP = "Cruz", ApellidoM = "Medina", Correo = "Cmarcos@mail.com" },
                new DetallesUsuario { Id = 49, Nombres = "Carolina", ApellidoP = "Valdez", ApellidoM = "Ávila", Correo = "Vcarolina@mail.com" },
                new DetallesUsuario { Id = 50, Nombres = "Emiliano", ApellidoP = "Chávez", ApellidoM = "Rico", Correo = "Cemiliano@mail.com" },
                new DetallesUsuario { Id = 51, Nombres = "Vanessa", ApellidoP = "Aguirre", ApellidoM = "Zamora", Correo = "Avanessa@mail.com" },
                new DetallesUsuario { Id = 52, Nombres = "Guillermo", ApellidoP = "Ochoa", ApellidoM = "Carrasco", Correo = "Oguillermo@mail.com" },
                new DetallesUsuario { Id = 53, Nombres = "Verónica", ApellidoP = "Estrada", ApellidoM = "Camacho", Correo = "Everónica@mail.com" },
                new DetallesUsuario { Id = 54, Nombres = "Mauricio", ApellidoP = "Valenzuela", ApellidoM = "Villanueva", Correo = "Vmauricio@mail.com" },
                new DetallesUsuario { Id = 55, Nombres = "Julieta", ApellidoP = "Tapia", ApellidoM = "Espinoza", Correo = "Tjulieta@mail.com" },
                new DetallesUsuario { Id = 56, Nombres = "Pablo", ApellidoP = "Campos", ApellidoM = "Ponce", Correo = "Cpablo@mail.com" },
                new DetallesUsuario { Id = 57, Nombres = "Sara", ApellidoP = "Delgado", ApellidoM = "Valdés", Correo = "Dsara@mail.com" },
                new DetallesUsuario { Id = 58, Nombres = "Hugo", ApellidoP = "Ortiz", ApellidoM = "Valdivia", Correo = "Ohugo@mail.com" },
                new DetallesUsuario { Id = 59, Nombres = "Laura", ApellidoP = "Hernández", ApellidoM = "Palma", Correo = "Hlaura@mail.com" },
                new DetallesUsuario { Id = 60, Nombres = "Raúl", ApellidoP = "Rivas", ApellidoM = "Suárez", Correo = "Rraúl@mail.com" },
                new DetallesUsuario { Id = 61, Nombres = "Irene", ApellidoP = "Cordero", ApellidoM = "Montes", Correo = "Cirene@mail.com" },
                new DetallesUsuario { Id = 62, Nombres = "Alfredo", ApellidoP = "Meza", ApellidoM = "Montero", Correo = "Malfredo@mail.com" },
                new DetallesUsuario { Id = 63, Nombres = "Patricia", ApellidoP = "Marín", ApellidoM = "Téllez", Correo = "Mpatricia@mail.com" },
                new DetallesUsuario { Id = 64, Nombres = "Hernán", ApellidoP = "Rocha", ApellidoM = "Tovar", Correo = "Rhernán@mail.com" },
                new DetallesUsuario { Id = 65, Nombres = "Florencia", ApellidoP = "Domínguez", ApellidoM = "Soria", Correo = "Dflorencia@mail.com" },
                new DetallesUsuario { Id = 66, Nombres = "Vicente", ApellidoP = "Sepúlveda", ApellidoM = "Ordóñez", Correo = "Svicente@mail.com" },
                new DetallesUsuario { Id = 67, Nombres = "Lourdes", ApellidoP = "Olivares", ApellidoM = "Castañeda", Correo = "Olourdes@mail.com" },
                new DetallesUsuario { Id = 68, Nombres = "Fabio", ApellidoP = "Ríos", ApellidoM = "Lara", Correo = "Rfabio@mail.com" },
                new DetallesUsuario { Id = 69, Nombres = "Clara", ApellidoP = "Morales", ApellidoM = "Cuenca", Correo = "Mclara@mail.com" },
                new DetallesUsuario { Id = 70, Nombres = "Esteban", ApellidoP = "Soto", ApellidoM = "Barrios", Correo = "Sesteban@mail.com" },
                new DetallesUsuario { Id = 71, Nombres = "Amanda", ApellidoP = "Cabrera", ApellidoM = "Ramos", Correo = "Camanda@mail.com" },
                new DetallesUsuario { Id = 72, Nombres = "Rodrigo", ApellidoP = "Salinas", ApellidoM = "Ochoa", Correo = "Srodrigo@mail.com" },
                new DetallesUsuario { Id = 73, Nombres = "Teresa", ApellidoP = "Ramírez", ApellidoM = "Duarte", Correo = "Rteresa@mail.com" },
                new DetallesUsuario { Id = 74, Nombres = "Hugo", ApellidoP = "Ponce", ApellidoM = "Morales", Correo = "Phugo@mail.com" },
                new DetallesUsuario { Id = 75, Nombres = "Daniela", ApellidoP = "Ortiz", ApellidoM = "López", Correo = "Odaniela@mail.com" },
                new DetallesUsuario { Id = 76, Nombres = "Gonzalo", ApellidoP = "Villalobos", ApellidoM = "Cortés", Correo = "Vgonzalo@mail.com" },
                new DetallesUsuario { Id = 77, Nombres = "Beatriz", ApellidoP = "Delgado", ApellidoM = "Zúñiga", Correo = "Dbeatriz@mail.com" },
                new DetallesUsuario { Id = 78, Nombres = "Felipe", ApellidoP = "Morales", ApellidoM = "Rivera", Correo = "Mfelipe@mail.com" },
                new DetallesUsuario { Id = 79, Nombres = "Liliana", ApellidoP = "Vega", ApellidoM = "Ramírez", Correo = "Vliliana@mail.com" },
                new DetallesUsuario { Id = 80, Nombres = "Ignacio", ApellidoP = "Navarro", ApellidoM = "Arredondo", Correo = "Nignacio@mail.com" },
                new DetallesUsuario { Id = 81, Nombres = "Adrián", ApellidoP = "Miranda", ApellidoM = "Barrera", Correo = "Madrián@mail.com" },
                new DetallesUsuario { Id = 82, Nombres = "Teresa", ApellidoP = "Reyes", ApellidoM = "Montoya", Correo = "Rteresa@mail.com" },
                new DetallesUsuario { Id = 83, Nombres = "Martín", ApellidoP = "Vázquez", ApellidoM = "Vargas", Correo = "Vmartín@mail.com" },
                new DetallesUsuario { Id = 84, Nombres = "Elena", ApellidoP = "Ibáñez", ApellidoM = "Guzmán", Correo = "Ielena@mail.com" },
                new DetallesUsuario { Id = 85, Nombres = "Lucas", ApellidoP = "Carrillo", ApellidoM = "Castillo", Correo = "Clucas@mail.com" },
                new DetallesUsuario { Id = 86, Nombres = "Dolores", ApellidoP = "Fuentes", ApellidoM = "Cabrera", Correo = "Fdolores@mail.com" },
                new DetallesUsuario { Id = 87, Nombres = "Ricardo", ApellidoP = "Escobar", ApellidoM = "Rodríguez", Correo = "Ericardo@mail.com" },
                new DetallesUsuario { Id = 88, Nombres = "Susana", ApellidoP = "Herrera", ApellidoM = "Fernández", Correo = "Hsusana@mail.com" });
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
                .HasMaxLength(15)
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

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Fecha)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("fecha");
            entity.Property(e => e.UsuarioDetallesUsuarioId).HasColumnName("usuario_detallesUsuario_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.UsuarioDetallesUsuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioDetallesUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido__usuario___7A672E12");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido__usuario___797309D9");
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
            entity.Property(e => e.Dirección)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("dirección");
            entity.Property(e => e.Estatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreAtiende)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreAtiende");
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreEmpresa");
            entity.Property(e => e.Teléfono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("teléfono");

            entity.HasData(
                new Proveedor { Id = 1, NombreEmpresa = "ORGON", NombreAtiende ="Julian", ApellidoM ="Perez", ApellidoP="Mariel" , Dirección = "Jose Maria Morelos, 110", Estatus = 1, Teléfono = "12345678"});
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__receta__3213E83F99B7B827");

            entity.ToTable("receta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Nombrelampara)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombrelampara");
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

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.DetallesUsuarioId).HasColumnName("detallesUsuario_id");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Token)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasOne(d => d.DetallesUsuario).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.DetallesUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario__detalle__3C69FB99");

            entity.HasData(
                new Usuario { Id = 1, Nombre = "alexa", Contraseña = "12345678", Rol = "Administrador", Estatus = 1, DetallesUsuarioId = 1, },
                new Usuario { Id = 2, Nombre = "ramirez", Contraseña = "12345678", Rol = "Administrador", Estatus = 1, DetallesUsuarioId = 2, },
                new Usuario { Id = 3, Nombre = "almeida", Contraseña = "12345678", Rol = "Administrador", Estatus = 1, DetallesUsuarioId = 3, },
                new Usuario { Id = 4, Nombre = "chekho", Contraseña = "12345678", Rol = "Administrador", Estatus = 1, DetallesUsuarioId = 4, },
                new Usuario { Id = 5, Nombre = "dario", Contraseña = "12345678", Rol = "Administrador", Estatus = 1, DetallesUsuarioId = 5, },

                new Usuario { Id = 6, Nombre = "Pjuancarlos", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 6, },
                new Usuario { Id = 7, Nombre = "Gmaríafernanda", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 7, },
                new Usuario { Id = 8, Nombre = "Rjoséluis", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 8, },
                new Usuario { Id = 9, Nombre = "Ranasofía", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 9, },
                new Usuario { Id = 10, Nombre = "Spedro", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 10, },
                new Usuario { Id = 11, Nombre = "Mgabriela", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 11, },
                new Usuario { Id = 12, Nombre = "Vcarlos", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 12, },
                new Usuario { Id = 13, Nombre = "Csofía", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 13, },
                new Usuario { Id = 14, Nombre = "Oluisfernando", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 14, },
                new Usuario { Id = 15, Nombre = "Jvaleria", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 15, },
                new Usuario { Id = 16, Nombre = "Hjavier", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 16, },
                new Usuario { Id = 17, Nombre = "Adaniela", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 17, },
                new Usuario { Id = 18, Nombre = "Gmiguelángel", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 18, },
                new Usuario { Id = 19, Nombre = "Snatalia", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 19, },
                new Usuario { Id = 20, Nombre = "Mandrés", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 20, },
                new Usuario { Id = 21, Nombre = "Npaola", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 21, },
                new Usuario { Id = 22, Nombre = "Rsebastián", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 22, },
                new Usuario { Id = 23, Nombre = "Llorena", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 23, },
                new Usuario { Id = 24, Nombre = "Iricardo", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 24, },
                new Usuario { Id = 25, Nombre = "Rteresa", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 25, },
                new Usuario { Id = 26, Nombre = "Feduardo", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 26, },
                new Usuario { Id = 27, Nombre = "Sadriana", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 27, },
                new Usuario { Id = 28, Nombre = "Cfrancisco", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 28, },
                new Usuario { Id = 29, Nombre = "Mmarcela", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 29, },
                new Usuario { Id = 30, Nombre = "Fsantiago", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 30, },
                new Usuario { Id = 31, Nombre = "Cisabel", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 31, },
                new Usuario { Id = 32, Nombre = "Bdiego", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 32, },
                new Usuario { Id = 33, Nombre = "Rfernanda", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 33, },
                new Usuario { Id = 34, Nombre = "Ealejandro", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 34, },
                new Usuario { Id = 35, Nombre = "Vcecilia", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 35, },
                new Usuario { Id = 36, Nombre = "Mmatías", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 36, },
                new Usuario { Id = 37, Nombre = "Fregina", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 37, },
                new Usuario { Id = 38, Nombre = "Mleonardo", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 38, },
                new Usuario { Id = 39, Nombre = "Pmiriam", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 39, },
                new Usuario { Id = 40, Nombre = "Ctomás", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 40, },
                new Usuario { Id = 41, Nombre = "Valejandra", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 41, },
                new Usuario { Id = 42, Nombre = "Pesteban", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 42, },
                new Usuario { Id = 43, Nombre = "Aandrea", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 43, },
                new Usuario { Id = 44, Nombre = "Rbenjamín", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 44, },
                new Usuario { Id = 45, Nombre = "Psofía", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 45, },
                new Usuario { Id = 46, Nombre = "Sjoaquín", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 46, },
                new Usuario { Id = 47, Nombre = "Mclaudia", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 47, },
                new Usuario { Id = 48, Nombre = "Cmarcos", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 48, },
                new Usuario { Id = 49, Nombre = "Vcarolina", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 49, },
                new Usuario { Id = 50, Nombre = "Cemiliano", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 50, },
                new Usuario { Id = 51, Nombre = "Avanessa", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 51, },
                new Usuario { Id = 52, Nombre = "Oguillermo", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 52, },
                new Usuario { Id = 53, Nombre = "Everónica", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 53, },
                new Usuario { Id = 54, Nombre = "Vmauricio", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 54, },
                new Usuario { Id = 55, Nombre = "Tjulieta", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 55, },
                new Usuario { Id = 56, Nombre = "Cpablo", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 56, },
                new Usuario { Id = 57, Nombre = "Dsara", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 57, },
                new Usuario { Id = 58, Nombre = "Ohugo", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 58, },
                new Usuario { Id = 59, Nombre = "Hlaura", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 59, },
                new Usuario { Id = 60, Nombre = "Rraúl", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 60, },
                new Usuario { Id = 61, Nombre = "Cirene", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 61, },
                new Usuario { Id = 62, Nombre = "Malfredo", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 62, },
                new Usuario { Id = 63, Nombre = "Mpatricia", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 63, },
                new Usuario { Id = 64, Nombre = "Rhernán", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 64, },
                new Usuario { Id = 65, Nombre = "Dflorencia", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 65, },
                new Usuario { Id = 66, Nombre = "Svicente", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 66, },
                new Usuario { Id = 67, Nombre = "Olourdes", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 67, },
                new Usuario { Id = 68, Nombre = "Rfabio", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 68, },
                new Usuario { Id = 69, Nombre = "Mclara", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 69, },
                new Usuario { Id = 70, Nombre = "Sesteban", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 70, },
                new Usuario { Id = 71, Nombre = "Camanda", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 71, },
                new Usuario { Id = 72, Nombre = "Srodrigo", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 72, },
                new Usuario { Id = 73, Nombre = "Rteresa", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 73, },
                new Usuario { Id = 74, Nombre = "Phugo", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 74, },
                new Usuario { Id = 75, Nombre = "Odaniela", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 75, },
                new Usuario { Id = 76, Nombre = "Vgonzalo", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 76, },
                new Usuario { Id = 77, Nombre = "Dbeatriz", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 77, },
                new Usuario { Id = 78, Nombre = "Mfelipe", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 78, },
                new Usuario { Id = 79, Nombre = "Vliliana", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 79, },
                new Usuario { Id = 80, Nombre = "Nignacio", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 80, },
                new Usuario { Id = 81, Nombre = "Madrián", Contraseña = "12345678A", Rol = "Almacén", Estatus = 1, DetallesUsuarioId = 81, },
                new Usuario { Id = 82, Nombre = "Rteresa", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 82, },
                new Usuario { Id = 83, Nombre = "Vmartín", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 83, },
                new Usuario { Id = 84, Nombre = "Ielena", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 84, },
                new Usuario { Id = 85, Nombre = "Clucas", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 85, },
                new Usuario { Id = 86, Nombre = "Fdolores", Contraseña = "12345678A", Rol = "Producción", Estatus = 1, DetallesUsuarioId = 86, },
                new Usuario { Id = 87, Nombre = "Ericardo", Contraseña = "12345678A", Rol = "Cliente", Estatus = 1, DetallesUsuarioId = 87, },
                new Usuario { Id = 88, Nombre = "Hsusana", Contraseña = "12345678A", Rol = "Logística", Estatus = 1, DetallesUsuarioId = 88, }
                );
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
