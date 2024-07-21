using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FarolitoAPIs.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Usuario> userManager)
        {
            using var context = new FarolitoDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FarolitoDbContext>>());
            // Catálogo de componentes
            if (!context.Componentes.Any())
            {
                context.Componentes.AddRange(
                    new Componente { Nombre = "cable" },
                    new Componente { Nombre = "regleta de conexión" },
                    new Componente { Nombre = "antitirones" },
                    new Componente { Nombre = "Tuerca paso 10/100" },
                    new Componente { Nombre = "Rosetón" },
                    new Componente { Nombre = "Florón" },
                    new Componente { Nombre = "remate" },
                    new Componente { Nombre = "Prensacables" },
                    new Componente { Nombre = "Portalámparas" },
                    new Componente { Nombre = "casquillo" },
                    new Componente { Nombre = "arandela" },
                    new Componente { Nombre = "pantalla" },
                    new Componente { Nombre = "Bombilla" }
                );
                context.SaveChanges();

            }
            var roles = new[] { "Administrador", "Cliente", "Logística", "Almacén", "Producción" };

            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    var identityRole = new IdentityRole(role)
                    {
                        NormalizedName = role.ToUpper() // Establece el NormalizedName
                    };
                    await context.Roles.AddAsync(identityRole);
                }
            }
            await context.SaveChangesAsync();




            // Crear usuario si no existe
            var users = new[]
            {
                new Usuario { Id = "1", UserName = "alexa@mail.com", Email = "alexa@mail.com", EmailConfirmed = true, FullName = "Alexa Guerrero López" },
                new Usuario { Id = "2", UserName = "almeida@mail.com", Email = "almeida@mail.com", EmailConfirmed = true, FullName = "Jose Angel Ramirez Almeida" },
                new Usuario { Id = "3", UserName = "angel@mail.com", Email = "angel@mail.com", EmailConfirmed = true, FullName = "Angel Eduardo Juarez Alvizo" },
                new Usuario { Id = "4", UserName = "sergiocecyteg@gmail.com", Email = "sergiocecyteg@gmail.com", EmailConfirmed = true, FullName = "Sergio de Jesús Salazar Cabrera" },
                new Usuario { Id = "5", UserName = "adriandario@mail.com", Email = "adriandario@mail.com", EmailConfirmed = true, FullName = "Adrián Darío Bravo Luna" },
                new Usuario { Id = "6", UserName = "Pjuancarlos@mail.com", Email = "Pjuancarlos@mail.com", EmailConfirmed = true, FullName = "Juan Carlos Pérez López" },
                new Usuario { Id = "7", UserName = "Gmaríafernanda@mail.com", Email = "Gmaríafernanda@mail.com", EmailConfirmed = true, FullName = "María Fernanda González Martínez" },
                new Usuario { Id = "8", UserName = "Vcarlos@mail.com", Email = "Vcarlos@mail.com", EmailConfirmed = true, FullName = "Carlos Vargas Mendoza" },
                new Usuario { Id = "9", UserName = "Nluisa@mail.com", Email = "Nluisa@mail.com", EmailConfirmed = true, FullName = "Luisa Navarro Ortiz" },
                new Usuario { Id = "10", UserName = "Jmanuel@mail.com", Email = "Jmanuel@mail.com", EmailConfirmed = true, FullName = "Manuel Jiménez Flores" },
                new Usuario { Id = "11", UserName = "Rjoséluis@mail.com", Email = "Rjoséluis@mail.com", EmailConfirmed = true, FullName = "José Luis Rodríguez Hernández" },
                new Usuario { Id = "12", UserName = "Ranasofía@mail.com", Email = "Ranasofía@mail.com", EmailConfirmed = true, FullName = "Ana Sofía Ramírez Torres" },
                new Usuario { Id = "13", UserName = "Oluisfernando@mail.com", Email = "Oluisfernando@mail.com", EmailConfirmed = true, FullName = "Luis Fernando Ortiz Delgado" },
                new Usuario { Id = "14", UserName = "Adaniela@mail.com", Email = "Adaniela@mail.com", EmailConfirmed = true, FullName = "Daniela Aguilar Morales" },
                new Usuario { Id = "15", UserName = "Snatalia@mail.com", Email = "Snatalia@mail.com", EmailConfirmed = true, FullName = "Natalia Soto Ruiz" },
                new Usuario { Id = "16", UserName = "Mandrés@mail.com", Email = "Mandrés@mail.com", EmailConfirmed = true, FullName = "Andrés Molina Castro" },
                new Usuario { Id = "17", UserName = "Npaola@mail.com", Email = "Npaola@mail.com", EmailConfirmed = true, FullName = "Paola Núñez Gómez" },
                new Usuario { Id = "18", UserName = "Spedro@mail.com", Email = "Spedro@mail.com", EmailConfirmed = true, FullName = "Pedro Sánchez Díaz" },
                new Usuario { Id = "19", UserName = "Mgabriela@mail.com", Email = "Mgabriela@mail.com", EmailConfirmed = true, FullName = "Gabriela Moreno García" },
                new Usuario { Id = "20", UserName = "Csofía@mail.com", Email = "Csofía@mail.com", EmailConfirmed = true, FullName = "Sofía Castillo Romero" },
                new Usuario { Id = "21", UserName = "Jvaleria@mail.com", Email = "Jvaleria@mail.com", EmailConfirmed = true, FullName = "Valeria Jiménez Silva" },
                new Usuario { Id = "22", UserName = "Hjavier@mail.com", Email = "Hjavier@mail.com", EmailConfirmed = true, FullName = "Javier Herrera Cruz" },
                new Usuario { Id = "23", UserName = "Gmiguelángel@mail.com", Email = "Gmiguelángel@mail.com", EmailConfirmed = true, FullName = "Miguel Ángel Guzmán Flores" },
                new Usuario { Id = "24", UserName = "Rsebastián@mail.com", Email = "Rsebastián@mail.com", EmailConfirmed = true, FullName = "Sebastián Ríos Domínguez" },
                new Usuario { Id = "25", UserName = "Llorena@mail.com", Email = "Llorena@mail.com", EmailConfirmed = true, FullName = "Lorena López Ortiz" },
                new Usuario { Id = "26", UserName = "Halejandro@mail.com", Email = "Halejandro@mail.com", EmailConfirmed = true, FullName = "Alejandro Hernández Cortés" },
                new Usuario { Id = "27", UserName = "Fjulia@mail.com", Email = "Fjulia@mail.com", EmailConfirmed = true, FullName = "Julia Fernández Serrano" },
                new Usuario { Id = "28", UserName = "Tguadalupe@mail.com", Email = "Tguadalupe@mail.com", EmailConfirmed = true, FullName = "Guadalupe Torres Ramos" },
                new Usuario { Id = "29", UserName = "Bmiguel@mail.com", Email = "Bmiguel@mail.com", EmailConfirmed = true, FullName = "Miguel Bautista Navarro" },
                new Usuario { Id = "30", UserName = "Dricardo@mail.com", Email = "Dricardo@mail.com", EmailConfirmed = true, FullName = "Ricardo Díaz Molina" },
                new Usuario { Id = "31", UserName = "Lfrancisco@mail.com", Email = "Lfrancisco@mail.com", EmailConfirmed = true, FullName = "Francisco Luna Cabrera" },
                new Usuario { Id = "32", UserName = "Mjosé@mail.com", Email = "Mjosé@mail.com", EmailConfirmed = true, FullName = "José Martínez González" },
                new Usuario { Id = "33", UserName = "Ejavier@mail.com", Email = "Ejavier@mail.com", EmailConfirmed = true, FullName = "Javier Escobar Pérez" },
                new Usuario { Id = "34", UserName = "Ladriana@mail.com", Email = "Ladriana@mail.com", EmailConfirmed = true, FullName = "Adriana López Hernández" },
                new Usuario { Id = "35", UserName = "Dfernando@mail.com", Email = "Dfernando@mail.com", EmailConfirmed = true, FullName = "Fernando Domínguez Sánchez" },
                new Usuario { Id = "36", UserName = "Mmariana@mail.com", Email = "Mmariana@mail.com", EmailConfirmed = true, FullName = "Mariana Martínez López" },
                new Usuario { Id = "37", UserName = "Fgustavo@mail.com", Email = "Fgustavo@mail.com", EmailConfirmed = true, FullName = "Gustavo Flores Ramos" }
            };

            var userRoles = new Dictionary<string, string>
        {
            { "alexa@mail.com", "Administrador" },
            { "almeida@mail.com", "Administrador" },
            { "angel@mail.com", "Administrador" },
            { "chekho@mail.com", "Administrador" },
            { "adriandario@mail.com", "Administrador" },
            { "Pjuancarlos@mail.com", "Cliente" },
            { "Gmaríafernanda@mail.com", "Cliente" },
            { "Vcarlos@mail.com", "Cliente" },
            { "Nluisa@mail.com", "Cliente" },
            { "Jmanuel@mail.com", "Cliente" },
            { "Rjoséluis@mail.com", "Logística" },
            { "Ranasofía@mail.com", "Producción" },
            { "Oluisfernando@mail.com", "Almacén" },
            { "Adaniela@mail.com", "Logística" },
            { "Snatalia@mail.com", "Almacén" },
            { "Mandrés@mail.com", "Almacén" },
            { "Npaola@mail.com", "Producción" },
            { "Spedro@mail.com", "Cliente" },
            { "Mgabriela@mail.com", "Cliente" },
            { "Csofía@mail.com", "Cliente" },
            { "Jvaleria@mail.com", "Cliente" },
            { "Hjavier@mail.com", "Cliente" },
            { "Gmiguelángel@mail.com", "Cliente" },
            { "Rsebastián@mail.com", "Cliente" },
            { "Llorena@mail.com", "Cliente" },
            { "Halejandro@mail.com", "Cliente" },
            { "Fjulia@mail.com", "Cliente" },
            { "Tguadalupe@mail.com", "Cliente" },
            { "Bmiguel@mail.com", "Cliente" },
            { "Dricardo@mail.com", "Cliente" },
            { "Lfrancisco@mail.com", "Cliente" },
            { "Mjosé@mail.com", "Cliente" },
            { "Ejavier@mail.com", "Cliente" },
            { "Ladriana@mail.com", "Cliente" },
            { "Dfernando@mail.com", "Cliente" },
            { "Mmariana@mail.com", "Cliente" },
            { "Fgustavo@mail.com", "Cliente" }
        };

              foreach (var user in users)
        {
            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                await userManager.CreateAsync(user, "Password123!");
            }
        }

        foreach (var userRole in userRoles)
        {
            var user = await userManager.FindByNameAsync(userRole.Key);
            if (user != null)
            {
                var role = userRole.Value;

                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }



            // Registros de proveedores c:
            if (!context.Proveedors.Any())
            {
                context.Proveedors.AddRange(
                    new Proveedor { NombreEmpresa = "ORGON", NombreAtiende = "Julian", ApellidoM = "Perez", ApellidoP = "Mariel", Dirección = "Jose Maria Morelos, 110", Estatus = 1, Teléfono = "12345678" },
                    new Proveedor { NombreEmpresa = "Steren", NombreAtiende = "Juan Carlos", ApellidoM = "Pérez", ApellidoP = "López", Dirección = "Avenida Insurgentes Sur 3500, Coyoacán, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5604 3578" },
                    new Proveedor { NombreEmpresa = "Gonher Proveedores", NombreAtiende = "María Fernanda", ApellidoM = "González", ApellidoP = "Martínez", Dirección = "Avenida Mariano Escobedo 151, Anáhuac I Secc, Miguel Hidalgo, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5580 6000" },
                    new Proveedor { NombreEmpresa = "Casa de las Lámparas", NombreAtiende = "José Luis", ApellidoM = "Rodríguez", ApellidoP = "Hernández", Dirección = "Isabel la Católica 36, Centro Histórico, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5512 1398" },
                    new Proveedor { NombreEmpresa = "Distribuidora Eléctrica Mexicana", NombreAtiende = "Ana Sofía", ApellidoM = "Ramírez", ApellidoP = "Torres", Dirección = "Calzada de Tlalpan 2735, Xotepingo, Coyoacán, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5601 2105" },
                    new Proveedor { NombreEmpresa = "Electrónica González", NombreAtiende = "Pedro", ApellidoM = "Sánchez", ApellidoP = "Díaz", Dirección = "Calle Victoria 57, Centro, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5512 0594" },
                    new Proveedor { NombreEmpresa = "Lámparas y Más", NombreAtiende = "Gabriela", ApellidoM = "Moreno", ApellidoP = "García", Dirección = "Avenida Revolución 130, Tacubaya, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5272 3280" },
                    new Proveedor { NombreEmpresa = "Conectores y Componentes", NombreAtiende = "Carlos", ApellidoM = "Vargas", ApellidoP = "Mendoza", Dirección = "Av. del Taller 49, Transito, Cuauhtémoc, Ciudad de México, CDMX", Estatus = 1, Teléfono = "52 55 5578 4001" }
                );

                context.SaveChanges();
            }

            // Relación productos(componentes)/proveedor(proveedor)
            if (!context.Productoproveedors.Any())
            {
                context.Productoproveedors.AddRange(
                    new Productoproveedor { ProveedorId = 1, ComponentesId = 3 },
                    new Productoproveedor { ProveedorId = 1, ComponentesId = 4 },
                    new Productoproveedor { ProveedorId = 1, ComponentesId = 7 },
                    new Productoproveedor { ProveedorId = 1, ComponentesId = 11 },
                    new Productoproveedor { ProveedorId = 1, ComponentesId = 13 },
                    new Productoproveedor { ProveedorId = 2, ComponentesId = 3 },
                    new Productoproveedor { ProveedorId = 2, ComponentesId = 4 },
                    new Productoproveedor { ProveedorId = 2, ComponentesId = 7 },
                    new Productoproveedor { ProveedorId = 2, ComponentesId = 13 },
                    new Productoproveedor { ProveedorId = 3, ComponentesId = 7 },
                    new Productoproveedor { ProveedorId = 3, ComponentesId = 8 },
                    new Productoproveedor { ProveedorId = 3, ComponentesId = 11 },
                    new Productoproveedor { ProveedorId = 4, ComponentesId = 4 },
                    new Productoproveedor { ProveedorId = 4, ComponentesId = 5 },
                    new Productoproveedor { ProveedorId = 4, ComponentesId = 8 },
                    new Productoproveedor { ProveedorId = 5, ComponentesId = 1 },
                    new Productoproveedor { ProveedorId = 5, ComponentesId = 8 },
                    new Productoproveedor { ProveedorId = 5, ComponentesId = 9 },
                    new Productoproveedor { ProveedorId = 5, ComponentesId = 10 },
                    new Productoproveedor { ProveedorId = 5, ComponentesId = 12 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 1 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 7 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 8 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 9 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 11 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 13 },
                    new Productoproveedor { ProveedorId = 7, ComponentesId = 1 },
                    new Productoproveedor { ProveedorId = 7, ComponentesId = 7 },
                    new Productoproveedor { ProveedorId = 7, ComponentesId = 9 },
                    new Productoproveedor { ProveedorId = 7, ComponentesId = 10 },
                    new Productoproveedor { ProveedorId = 7, ComponentesId = 11 },
                    new Productoproveedor { ProveedorId = 7, ComponentesId = 13 },
                    new Productoproveedor { ProveedorId = 8, ComponentesId = 5 },
                    new Productoproveedor { ProveedorId = 8, ComponentesId = 9 },
                    new Productoproveedor { ProveedorId = 8, ComponentesId = 11 },
                    new Productoproveedor { ProveedorId = 8, ComponentesId = 12 },

                    new Productoproveedor { ProveedorId = 3, ComponentesId = 2 },
                    new Productoproveedor { ProveedorId = 4, ComponentesId = 2 },
                    new Productoproveedor { ProveedorId = 3, ComponentesId = 6 },
                    new Productoproveedor { ProveedorId = 6, ComponentesId = 6 }
                );
                context.SaveChanges();
            }

            // Relación productos(componentes)/proveedor(proveedor)
            if (!context.Receta.Any())
            {
                context.Receta.AddRange(
                    new Recetum { Nombrelampara = "Lámpara de mesa", Estatus = 1 },
                    new Recetum { Nombrelampara = "Lámpara de pie", Estatus = 1 },
                    new Recetum { Nombrelampara = "Lámpara de Techo", Estatus = 1 },
                    new Recetum { Nombrelampara = "Lámpara colgante", Estatus = 1 },
                    new Recetum { Nombrelampara = "Lámpara para exterior", Estatus = 1 }
                );
                context.SaveChanges();
            }

            // registros/Relación Detalles de recetas
            if (!context.Componentesreceta.Any())
            {
                context.Componentesreceta.AddRange(
                    //
                    new Componentesrecetum { ComponentesId = 1, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 2, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 3, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 4, RecetaId = 1, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 8, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 9, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 10, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 11, RecetaId = 1, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 12, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 13, RecetaId = 1, Cantidad = 1, Estatus = 1, },
                    //
                    new Componentesrecetum { ComponentesId = 1, RecetaId = 2, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 2, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 3, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 4, RecetaId = 2, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 8, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 9, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 10, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 11, RecetaId = 2, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 12, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 13, RecetaId = 2, Cantidad = 1, Estatus = 1, },
                    //
                    new Componentesrecetum { ComponentesId = 1, RecetaId = 3, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 2, RecetaId = 3, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 4, RecetaId = 3, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 5, RecetaId = 3, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 6, RecetaId = 3, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 9, RecetaId = 3, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 10, RecetaId = 3, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 11, RecetaId = 3, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 13, RecetaId = 3, Cantidad = 1, Estatus = 1, },
                    //
                    new Componentesrecetum { ComponentesId = 1, RecetaId = 4, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 2, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 4, RecetaId = 4, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 5, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 6, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 8, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 9, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 10, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 11, RecetaId = 4, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 13, RecetaId = 4, Cantidad = 1, Estatus = 1, },
                    //
                    new Componentesrecetum { ComponentesId = 1, RecetaId = 5, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 2, RecetaId = 5, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 3, RecetaId = 5, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 4, RecetaId = 5, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 8, RecetaId = 5, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 9, RecetaId = 5, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 10, RecetaId = 5, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 11, RecetaId = 5, Cantidad = 2, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 12, RecetaId = 5, Cantidad = 1, Estatus = 1, },
                    new Componentesrecetum { ComponentesId = 13, RecetaId = 5, Cantidad = 1, Estatus = 1, }
                );
                context.SaveChanges();
            }


            // pendiente: seeddata Inventario 
            if (!context.Compras.Any())
            {
                context.Compras.AddRange(
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 1),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {
                    new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROCAB-0000AUV"},
                    new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROCAB-0000SOL"},
                    new Detallecompra {Cantidad = 10, Costo = 200, Lote = "PROCAB-0000SIE"},
                    new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROCAB-0000NLC"},
                    new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROCAB-0000FFO"},
                }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 2),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> {
                    new Detallecompra {Cantidad = 6, Costo = 120, Lote = "PROCAB-0000AIA"},
                    new Detallecompra {Cantidad = 10, Costo = 200, Lote = "PROCAB-0000TFN"},
                    new Detallecompra {Cantidad = 6, Costo = 120, Lote = "PROCAB-0000GFY"},
                    new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROCAB-0000GDR"},
                    new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROCAB-0000SDB"},
                }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 3),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {
                    new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROCAB-0000PDV"},
                    new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROCAB-0000PIF"},
                    new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROCAB-0000GSY"},
                    new Detallecompra {Cantidad = 6, Costo = 120, Lote = "PROCAB-0000NVM"},
                    new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROCAB-0000AYE"},
                }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 4),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {
                    new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAB-0000OJB"},
                    new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROCAB-0000SDG"},
                    new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROCAB-0000RZO"},
                    new Detallecompra {Cantidad = 10, Costo = 200, Lote = "PROCAB-0000BDN"},
                    new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROCAB-0000JFP"},
                }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 5),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAB-0000AUV"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROCAB-0000MII"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROCAB-0000XUS"},
new Detallecompra {Cantidad = 6, Costo = 120, Lote = "PROCAB-0000IBQ"},
new Detallecompra {Cantidad = 9, Costo = 135, Lote = "PROREG-0000GMN"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 6),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREG-0000VJL"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREG-0000RTC"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROREG-0000ORS"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROREG-0000TJL"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROREG-0000IFU"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 7),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREG-0000OHF"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROREG-0000IID"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROREG-0000EDS"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREG-0000ALL"},
new Detallecompra {Cantidad = 9, Costo = 135, Lote = "PROREG-0000MGL"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 8),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROREG-0000KHG"},
new Detallecompra {Cantidad = 5, Costo = 75, Lote = "PROREG-0000QLM"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROREG-0000KAV"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREG-0000MGU"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREG-0000JKR"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 9),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 6, Costo = 90, Lote = "PROREG-0000VYD"},
new Detallecompra {Cantidad = 10, Costo = 150, Lote = "PROREG-0000KUG"},
new Detallecompra {Cantidad = 5, Costo = 75, Lote = "PROREG-0000QUT"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREG-0000NEN"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREG-0000YTK"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 10),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 10, Costo = 150, Lote = "PROREG-0000PFC"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROREG-0000OZF"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROREG-0000LQA"},
new Detallecompra {Cantidad = 6, Costo = 90, Lote = "PROREG-0000WDC"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREG-0000WON"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 11),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 5, Costo = 75, Lote = "PROREG-0000WZA"},
new Detallecompra {Cantidad = 6, Costo = 90, Lote = "PROREG-0000OID"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROREG-0000VFQ"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROANT-0000HOZ"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROANT-0000JSL"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 12),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROANT-0000SNC"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROANT-0000AMN"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROANT-0000YBX"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROANT-0000TZZ"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROANT-0000OST"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 13),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROANT-0000ZBR"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROANT-0000VDM"},
new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROANT-0000HLI"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROANT-0000KCE"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROANT-0000UJI"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 14),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROANT-0000IYC"},
new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROANT-0000GWL"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROANT-0000RPP"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROANT-0000VUG"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROANT-0000ERV"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 15),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROANT-0000PPL"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROANT-0000HNG"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROANT-0000DTI"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROANT-0000BKF"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROANT-0000HDP"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 16),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROANT-0000AIU"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROANT-0000OSM"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROANT-0000CPG"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROANT-0000PWE"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROANT-0000FWZ"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 17),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROANT-0000OHQ"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROANT-0000AQU"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROANT-0000TUE"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROANT-0000YBD"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROANT-0000ALV"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 18),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROANT-0000GNQ"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROANT-0000UAX"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROANT-0000ZTN"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROANT-0000SIH"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROANT-0000FWX"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 19),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROANT-0000WYS"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROANT-0000LPQ"},
new Detallecompra {Cantidad = 5, Costo = 5, Lote = "PROTUE-0000HUN"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROTUE-0000QXS"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROTUE-0000SCV"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 20),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROTUE-0000YMN"},
new Detallecompra {Cantidad = 6, Costo = 6, Lote = "PROTUE-0000ROG"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROTUE-0000HBJ"},
new Detallecompra {Cantidad = 8, Costo = 8, Lote = "PROTUE-0000FEL"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROTUE-0000WPZ"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 21),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROTUE-0000XOC"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROTUE-0000ZPT"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROTUE-0000QNV"},
new Detallecompra {Cantidad = 10, Costo = 10, Lote = "PROTUE-0000XTM"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROTUE-0000QVN"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 22),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 10, Costo = 10, Lote = "PROTUE-0000XQQ"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROTUE-0000UUG"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROTUE-0000IBL"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROTUE-0000VPM"},
new Detallecompra {Cantidad = 5, Costo = 5, Lote = "PROTUE-0000LGQ"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 23),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 9, Lote = "PROTUE-0000APO"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROTUE-0000OPL"},
new Detallecompra {Cantidad = 8, Costo = 8, Lote = "PROTUE-0000MHF"},
new Detallecompra {Cantidad = 5, Costo = 5, Lote = "PROTUE-0000ZLI"},
new Detallecompra {Cantidad = 8, Costo = 8, Lote = "PROTUE-0000GJG"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 24),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROTUE-0000GGM"},
new Detallecompra {Cantidad = 10, Costo = 10, Lote = "PROTUE-0000CGB"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROTUE-0000OMH"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROROS-0000LLB"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROROS-0000HXT"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 25),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROROS-0000DPK"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROROS-0000FPG"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROROS-0000CIB"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROROS-0000LTW"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROROS-0000KID"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 26),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROROS-0000KWI"},
new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROROS-0000GUE"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROROS-0000QKN"},
new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROROS-0000JZY"},
new Detallecompra {Cantidad = 6, Costo = 120, Lote = "PROROS-0000ZQD"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 27),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROROS-0000AFX"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROROS-0000SVK"},
new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROROS-0000ESC"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROROS-0000VFV"},
new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROROS-0000OVQ"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 28),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROROS-0000ZNS"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROROS-0000LJA"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROROS-0000WDG"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROROS-0000SGD"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROROS-0000KIQ"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 29),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 10, Costo = 200, Lote = "PROROS-0000ASX"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROROS-0000JNL"},
new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROROS-0000YSC"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROROS-0000WBE"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROROS-0000MVU"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 30),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROROS-0000FLO"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROROS-0000QYH"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROROS-0000OLN"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROROS-0000JRP"},
new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROFLO-0000BWY"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 5, 31),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROFLO-0000SDR"},
new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROFLO-0000VKE"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROFLO-0000GOE"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROFLO-0000LGS"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROFLO-0000CSD"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 1),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROFLO-0000FFX"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROFLO-0000UFU"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROFLO-0000SAN"},
new Detallecompra {Cantidad = 10, Costo = 200, Lote = "PROFLO-0000XZX"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROFLO-0000QRU"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 2),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROFLO-0000SQC"},
new Detallecompra {Cantidad = 6, Costo = 120, Lote = "PROFLO-0000IOE"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROFLO-0000CCI"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROFLO-0000QON"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROFLO-0000MGL"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 3),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROFLO-0000TPV"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROFLO-0000OZL"},
new Detallecompra {Cantidad = 10, Costo = 200, Lote = "PROFLO-0000QBS"},
new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROFLO-0000JIB"},
new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROFLO-0000DNO"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 4),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROFLO-0000BZM"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROFLO-0000EVS"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROFLO-0000PWV"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROFLO-0000BTC"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROFLO-0000CEZ"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 5),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROFLO-0000QDC"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROFLO-0000EEM"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROFLO-0000GRX"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROFLO-0000PPJ"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROFLO-0000YSG"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 6),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 10, Costo = 150, Lote = "PROREM-0000NKC"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROREM-0000KWO"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROREM-0000WCC"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROREM-0000RHN"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREM-0000PGL"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 7),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROREM-0000ZYW"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREM-0000EVD"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROREM-0000HNQ"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREM-0000ILZ"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREM-0000HJS"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 8),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROREM-0000RJB"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROREM-0000BYE"},
new Detallecompra {Cantidad = 5, Costo = 75, Lote = "PROREM-0000EDU"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROREM-0000WJT"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROREM-0000DIR"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 9),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREM-0000FUB"},
new Detallecompra {Cantidad = 6, Costo = 90, Lote = "PROREM-0000GNL"},
new Detallecompra {Cantidad = 10, Costo = 150, Lote = "PROREM-0000JIV"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROREM-0000ZIT"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREM-0000ERW"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 10),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROREM-0000WHQ"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROREM-0000BGY"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROREM-0000HXI"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROREM-0000NMX"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROREM-0000JWS"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 11),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROREM-0000QQJ"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROREM-0000GBK"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROPRE-0000HKN"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROPRE-0000NKX"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROPRE-0000VHF"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 12),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROPRE-0000TMV"},
new Detallecompra {Cantidad = 9, Costo = 135, Lote = "PROPRE-0000CHI"},
new Detallecompra {Cantidad = 10, Costo = 150, Lote = "PROPRE-0000BKJ"},
new Detallecompra {Cantidad = 6, Costo = 90, Lote = "PROPRE-0000MMA"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROPRE-0000ZPP"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 13),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROPRE-0000HZG"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROPRE-0000IOU"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROPRE-0000ODE"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROPRE-0000PCD"},
new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROPRE-0000DNF"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 14),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROPRE-0000IRS"},
new Detallecompra {Cantidad = 7, Costo = 105, Lote = "PROPRE-0000LHV"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROPRE-0000HMT"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROPRE-0000XAZ"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROPRE-0000ZCJ"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 15),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 75, Lote = "PROPRE-0000SYE"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROPRE-0000FKM"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROPRE-0000HYR"},
new Detallecompra {Cantidad = 8, Costo = 40, Lote = "PROPRE-0000JXE"},
new Detallecompra {Cantidad = 9, Costo = 135, Lote = "PROPRE-0000FNR"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 16),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 8, Costo = 120, Lote = "PROPRE-0000GYF"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROPRE-0000WUI"},
new Detallecompra {Cantidad = 9, Costo = 270, Lote = "PROPOR-0000TYA"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROPOR-0000BEV"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROPOR-0000VWL"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 17),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 210, Lote = "PROPOR-0000DWB"},
new Detallecompra {Cantidad = 5, Costo = 150, Lote = "PROPOR-0000LLI"},
new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROPOR-0000QZR"},
new Detallecompra {Cantidad = 10, Costo = 300, Lote = "PROPOR-0000XAM"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROPOR-0000FLO"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 18),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROPOR-0000WSB"},
new Detallecompra {Cantidad = 8, Costo = 240, Lote = "PROPOR-0000WST"},
new Detallecompra {Cantidad = 8, Costo = 240, Lote = "PROPOR-0000SPS"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROPOR-0000KCX"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROPOR-0000AOM"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 19),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 6, Costo = 180, Lote = "PROPOR-0000BOR"},
new Detallecompra {Cantidad = 10, Costo = 300, Lote = "PROPOR-0000JUK"},
new Detallecompra {Cantidad = 10, Costo = 300, Lote = "PROPOR-0000UCD"},
new Detallecompra {Cantidad = 10, Costo = 300, Lote = "PROPOR-0000XLX"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROPOR-0000VFN"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 20),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 270, Lote = "PROPOR-0000CJH"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROPOR-0000XVF"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROPOR-0000DOI"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROPOR-0000YLT"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROPOR-0000VID"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 21),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 150, Lote = "PROPOR-0000FIV"},
new Detallecompra {Cantidad = 9, Costo = 270, Lote = "PROPOR-0000AHJ"},
new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROPOR-0000ZRB"},
new Detallecompra {Cantidad = 8, Costo = 240, Lote = "PROPOR-0000QPV"},
new Detallecompra {Cantidad = 10, Costo = 300, Lote = "PROPOR-0000XPI"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 22),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROPOR-0000BXZ"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROPOR-0000TEJ"},
new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROPOR-0000OED"},
new Detallecompra {Cantidad = 5, Costo = 150, Lote = "PROPOR-0000EYB"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROPOR-0000NHY"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 23),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROCAS-0000EZL"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAS-0000SRM"},
new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROCAS-0000RGC"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROCAS-0000UAH"},
new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROCAS-0000ZNR"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 24),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROCAS-0000IHA"},
new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROCAS-0000YTV"},
new Detallecompra {Cantidad = 8, Costo = 160, Lote = "PROCAS-0000GTO"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROCAS-0000AOF"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROCAS-0000MRV"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 25),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAS-0000JUV"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAS-0000YAB"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROCAS-0000UJV"},
new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROCAS-0000XXN"},
new Detallecompra {Cantidad = 9, Costo = 180, Lote = "PROCAS-0000EYV"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 26),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROCAS-0000TSD"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROCAS-0000NZU"},
new Detallecompra {Cantidad = 5, Costo = 100, Lote = "PROCAS-0000SXH"},
new Detallecompra {Cantidad = 7, Costo = 140, Lote = "PROCAS-0000FBQ"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAS-0000ING"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 27),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROCAS-0000VPU"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROCAS-0000YQS"},
new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROCAS-0000KSF"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROCAS-0000HQQ"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROCAS-0000EME"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 28),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 9, Lote = "PROARA-0000QYF"},
new Detallecompra {Cantidad = 10, Costo = 10, Lote = "PROARA-0000API"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROARA-0000WIU"},
new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROARA-0000JSU"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROARA-0000JAG"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 29),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 5, Lote = "PROARA-0000HDJ"},
new Detallecompra {Cantidad = 7, Costo = 35, Lote = "PROARA-0000OOP"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROARA-0000LFJ"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROARA-0000MDR"},
new Detallecompra {Cantidad = 9, Costo = 45, Lote = "PROARA-0000KOG"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 6, 30),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 6, Costo = 30, Lote = "PROARA-0000YGD"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROARA-0000WYZ"},
new Detallecompra {Cantidad = 7, Costo = 7, Lote = "PROARA-0000RUD"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROARA-0000SHO"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROARA-0000URP"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 1),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 9, Lote = "PROARA-0000BBD"},
new Detallecompra {Cantidad = 10, Costo = 50, Lote = "PROARA-0000SLQ"},
new Detallecompra {Cantidad = 8, Costo = 8, Lote = "PROARA-0000XSI"},
new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROARA-0000IGY"},
new Detallecompra {Cantidad = 8, Costo = 8, Lote = "PROARA-0000GDS"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 2),
                    UsuarioId = "5",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 25, Lote = "PROARA-0000AEN"},
new Detallecompra {Cantidad = 5, Costo = 5, Lote = "PROARA-0000WPC"},
new Detallecompra {Cantidad = 6, Costo = 1200, Lote = "PROPAN-0000MSK"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROPAN-0000NUU"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROPAN-0000XQU"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 3),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 1400, Lote = "PROPAN-0000IUV"},
new Detallecompra {Cantidad = 6, Costo = 1200, Lote = "PROPAN-0000VKH"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROPAN-0000SBY"},
new Detallecompra {Cantidad = 9, Costo = 1800, Lote = "PROPAN-0000DYM"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROPAN-0000YLR"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 4),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROPAN-0000LTT"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROPAN-0000YRI"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROPAN-0000OSR"},
new Detallecompra {Cantidad = 8, Costo = 1600, Lote = "PROPAN-0000XEC"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROPAN-0000EOT"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 5),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROPAN-0000NST"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROPAN-0000FGM"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROPAN-0000RIE"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROPAN-0000GDL"},
new Detallecompra {Cantidad = 9, Costo = 1800, Lote = "PROPAN-0000ZBR"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 6),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 6, Costo = 1200, Lote = "PROPAN-0000OMM"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROPAN-0000TTU"},
new Detallecompra {Cantidad = 5, Costo = 1000, Lote = "PROPAN-0000ZIT"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROPAN-0000MJK"},
new Detallecompra {Cantidad = 7, Costo = 1400, Lote = "PROPAN-0000LON"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 7),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROPAN-0000DWO"},
new Detallecompra {Cantidad = 6, Costo = 1200, Lote = "PROPAN-0000MCT"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROPAN-0000CVK"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROBOM-0000RCQ"},
new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROBOM-0000JTN"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 8),
                    UsuarioId = "3",
                    Detallecompras = new List<Detallecompra> {new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROBOM-0000WVA"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROBOM-0000MCP"},
new Detallecompra {Cantidad = 6, Costo = 60, Lote = "PROBOM-0000PQS"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROBOM-0000QXZ"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROBOM-0000ZSI"}, }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 9),
                    UsuarioId = "1",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 7, Costo = 70, Lote = "PROBOM-0000NRK"},
new Detallecompra {Cantidad = 10, Costo = 500, Lote = "PROBOM-0000TSN"},
new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROBOM-0000IIA"},
new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROBOM-0000WBX"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROBOM-0000IXV"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 10),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 50, Lote = "PROBOM-0000LFV"},
new Detallecompra {Cantidad = 6, Costo = 300, Lote = "PROBOM-0000DLH"},
new Detallecompra {Cantidad = 9, Costo = 90, Lote = "PROBOM-0000SLT"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROBOM-0000HLP"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROBOM-0000GXJ"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 11),
                    UsuarioId = "4",
                    Detallecompras = new List<Detallecompra> {
                        new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROBOM-0000BVZ"},
                        new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROBOM-0000VOL"},
                        new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROBOM-0000LCI"},
                        new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROBOM-0000DEM"},
                        new Detallecompra {Cantidad = 8, Costo = 80, Lote = "PROBOM-0000BKN"},}
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 12),
                    UsuarioId = "2",
                    Detallecompras = new List<Detallecompra> { new Detallecompra {Cantidad = 5, Costo = 250, Lote = "PROBOM-0000DVO"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROBOM-0000TAD"},
new Detallecompra {Cantidad = 9, Costo = 450, Lote = "PROBOM-0000KXQ"},
new Detallecompra {Cantidad = 10, Costo = 100, Lote = "PROBOM-0000JNY"},
new Detallecompra {Cantidad = 7, Costo = 350, Lote = "PROBOM-0000OVQ"},
new Detallecompra {Cantidad = 8, Costo = 400, Lote = "PROBOM-0000HZB"},}
                },


                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 15),
                    UsuarioId = "13",
                    Detallecompras = new List<Detallecompra> {
                        new Detallecompra {Cantidad = 50, Costo = 550, Lote = "PROBOM-000TOPE"},
                        new Detallecompra {Cantidad = 50, Costo = 500, Lote = "PROBOM-0001TAD"},
                        new Detallecompra {Cantidad = 50, Costo = 250, Lote = "PROBOM-0001KXQ"},
                        new Detallecompra {Cantidad = 50, Costo = 50, Lote = "PROBOM-0001JNY"},
                        new Detallecompra {Cantidad = 50, Costo = 1000, Lote = "PROBOM-0001OVQ"},
                        new Detallecompra {Cantidad = 50, Costo = 1000, Lote = "PROBOM-0001HZB"}
                    }
                },
                new Compra
                {
                    Fecha = new DateOnly(2024, 7, 16),
                    UsuarioId = "15",
                    Detallecompras = new List<Detallecompra> {
                        new Detallecompra {Cantidad = 50, Costo = 250, Lote = "PROBOM-0002OPE"},
                        new Detallecompra {Cantidad = 50, Costo = 250, Lote = "PROBOM-0002TAD"},
                        new Detallecompra {Cantidad = 50, Costo = 500, Lote = "PROBOM-0002KXQ"},
                        new Detallecompra {Cantidad = 50, Costo = 100, Lote = "PROBOM-0002JNY"},
                        new Detallecompra {Cantidad = 50, Costo = 50, Lote = "PROBOM-0002OVQ"},
                        new Detallecompra {Cantidad = 50, Costo = 2500, Lote = "PROBOM-0002HZB"},
                        new Detallecompra {Cantidad = 50, Costo = 500, Lote = "PROBOM-0002HAB"}
                    }
                }
                );

                context.SaveChanges();
            }


            if (!context.Inventariocomponentes.Any())
            {
                context.Inventariocomponentes.AddRange(
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 1 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 2 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 3 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 4 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 5 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 6 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 7 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 8 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 9 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 10 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 11 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 12 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 13 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 14 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 15 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 16 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 17 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 18 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 19 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 20 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 21 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 22 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 1, DetallecompraId = 23 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 7, ComponentesId = 1, DetallecompraId = 24 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 25 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 26 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 27 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 28 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 29 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 30 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 31 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 32 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 33 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 34 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 35 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 36 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 37 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 38 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 39 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 40 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 41 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 42 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 43 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 44 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 45 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 46 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 2, DetallecompraId = 47 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 48 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 49 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 50 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 51 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 52 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 53 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 54 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 55 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 56 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 57 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 58 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 59 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 60 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 61 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 62 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 63 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 64 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 65 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 66 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 67 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 68 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 69 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 70 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 71 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 72 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 73 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 74 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 75 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 76 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 77 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 78 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 79 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 80 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 81 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 82 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 83 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 84 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 85 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 86 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 87 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 88 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 89 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 90 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 2, ComponentesId = 3, DetallecompraId = 91 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 92 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 93 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 94 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 95 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 96 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 97 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 98 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 99 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 100 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 101 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 102 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 103 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 104 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 105 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 106 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 107 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 108 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 109 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 110 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 111 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 112 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 113 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 114 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 115 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 4, DetallecompraId = 116 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 117 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 4, DetallecompraId = 118 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 119 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 120 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 121 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 122 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 123 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 124 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 125 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 126 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 127 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 128 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 129 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 130 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 131 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 132 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 133 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 134 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 135 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 136 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 137 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 138 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 139 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 140 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 141 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 142 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 143 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 144 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 145 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 146 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 147 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 148 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 5, DetallecompraId = 149 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 150 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 151 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 152 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 153 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 154 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 155 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 156 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 157 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 158 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 159 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 160 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 161 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 162 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 163 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 164 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 165 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 166 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 167 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 168 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 169 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 170 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 171 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 172 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 173 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 174 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 175 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 176 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 177 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 178 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 6, ComponentesId = 6, DetallecompraId = 179 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 180 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 181 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 7, DetallecompraId = 182 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 7, DetallecompraId = 183 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 7, DetallecompraId = 184 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 7, DetallecompraId = 185 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 186 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 7, DetallecompraId = 187 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 7, DetallecompraId = 188 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 7, DetallecompraId = 189 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 7, ComponentesId = 7, DetallecompraId = 190 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 191 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 7, DetallecompraId = 192 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 7, DetallecompraId = 193 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 7, DetallecompraId = 194 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 7, DetallecompraId = 195 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 196 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 2, ComponentesId = 7, DetallecompraId = 197 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 7, DetallecompraId = 198 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 7, DetallecompraId = 199 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 7, ComponentesId = 7, DetallecompraId = 200 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 201 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 7, DetallecompraId = 202 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 3, ComponentesId = 7, DetallecompraId = 203 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 6, ComponentesId = 7, DetallecompraId = 204 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 7, DetallecompraId = 205 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 206 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 7, DetallecompraId = 207 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 208 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 209 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 8, DetallecompraId = 210 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 8, DetallecompraId = 211 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 212 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 213 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 8, DetallecompraId = 214 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 8, DetallecompraId = 215 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 216 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 217 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 5, ComponentesId = 8, DetallecompraId = 218 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 8, DetallecompraId = 219 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 220 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 221 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 8, DetallecompraId = 222 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 8, DetallecompraId = 223 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 224 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 225 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 8, DetallecompraId = 226 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 8, DetallecompraId = 227 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 228 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 229 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 8, DetallecompraId = 230 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 8, DetallecompraId = 231 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 4, ComponentesId = 8, DetallecompraId = 232 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 233 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 234 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 235 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 236 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 237 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 238 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 239 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 240 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 241 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 242 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 243 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 244 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 245 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 246 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 247 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 248 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 249 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 250 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 251 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 252 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 253 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 254 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 255 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 256 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 257 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 258 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 259 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 260 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 261 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 6, ComponentesId = 9, DetallecompraId = 262 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 7, ComponentesId = 9, DetallecompraId = 263 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 8, ComponentesId = 9, DetallecompraId = 264 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 265 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 266 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 267 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 268 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 269 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 270 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 271 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 272 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 273 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 274 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 275 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 276 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 277 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 278 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 279 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 280 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 281 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 282 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 283 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 284 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 285 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 286 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 287 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 288 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 10, DetallecompraId = 289 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 290 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 11, DetallecompraId = 291 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 11, DetallecompraId = 292 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 6, ComponentesId = 11, DetallecompraId = 293 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 7, ComponentesId = 11, DetallecompraId = 294 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 11, DetallecompraId = 295 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 11, DetallecompraId = 296 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 3, ComponentesId = 11, DetallecompraId = 297 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 11, DetallecompraId = 298 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 11, DetallecompraId = 299 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 11, DetallecompraId = 300 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 1, ComponentesId = 11, DetallecompraId = 301 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 3, ComponentesId = 11, DetallecompraId = 302 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 11, DetallecompraId = 303 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 11, DetallecompraId = 304 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 8, ComponentesId = 11, DetallecompraId = 305 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 11, DetallecompraId = 306 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 3, ComponentesId = 11, DetallecompraId = 307 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 11, DetallecompraId = 308 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 11, DetallecompraId = 309 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 8, ComponentesId = 11, DetallecompraId = 310 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 11, DetallecompraId = 311 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 7, ComponentesId = 11, DetallecompraId = 312 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 313 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 314 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 315 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 316 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 317 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 318 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 319 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 320 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 321 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 322 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 323 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 324 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 325 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 326 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 327 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 328 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 329 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 330 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 331 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 332 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 333 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 334 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 335 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 336 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 337 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 8, ComponentesId = 12, DetallecompraId = 338 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 339 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 340 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 341 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 342 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 343 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 344 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 345 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 346 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 347 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 348 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 349 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 350 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 351 },
                    new Inventariocomponente { Cantidad = 6, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 352 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 353 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 354 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 355 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 356 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 357 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 358 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 359 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 360 },
                    new Inventariocomponente { Cantidad = 5, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 361 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 362 },
                    new Inventariocomponente { Cantidad = 9, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 363 },
                    new Inventariocomponente { Cantidad = 10, ProveedorId = 2, ComponentesId = 13, DetallecompraId = 364 },
                    new Inventariocomponente { Cantidad = 7, ProveedorId = 6, ComponentesId = 13, DetallecompraId = 365 },
                    new Inventariocomponente { Cantidad = 8, ProveedorId = 7, ComponentesId = 13, DetallecompraId = 366 },

                    new Inventariocomponente { Cantidad = 50, ProveedorId = 5, ComponentesId = 1, DetallecompraId = 367 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 3, ComponentesId = 2, DetallecompraId = 368 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 1, ComponentesId = 3, DetallecompraId = 369 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 1, ComponentesId = 4, DetallecompraId = 370 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 4, ComponentesId = 5, DetallecompraId = 371 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 3, ComponentesId = 6, DetallecompraId = 372 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 1, ComponentesId = 7, DetallecompraId = 373 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 3, ComponentesId = 8, DetallecompraId = 374 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 5, ComponentesId = 9, DetallecompraId = 375 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 5, ComponentesId = 10, DetallecompraId = 376 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 1, ComponentesId = 11, DetallecompraId = 377 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 5, ComponentesId = 12, DetallecompraId = 378 },
                    new Inventariocomponente { Cantidad = 50, ProveedorId = 1, ComponentesId = 13, DetallecompraId = 379 }
                );
                context.SaveChanges();
            }

            if (context.Inventariolamparas.Any())
            {
                return;
            }
        }
    }
}