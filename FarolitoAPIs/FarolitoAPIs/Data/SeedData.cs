using FarolitoAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Data
{    public static class SeedData 
    {        public static void Initialize(IServiceProvider serviceProvider)
        {            using var context = new FarolitoDbContext(
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

            // Registros de usuarios
            /*if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange();
            }*/

            // Registros de proveedores c:
            if (!context.Proveedors.Any()){
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
                    new Productoproveedor { ProveedorId = 8, ComponentesId = 12 }
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
        }
    }    
}