namespace FarolitoAPIs.Controllers
{
    public class LotesVentas
    {
        public static string GenerarLote(DateTime fechaVenta)
        {
            string iniciales = "VLH-";
            // Formatear la fecha
            string fechaFormato = $"{fechaVenta:yyMMdd}"; // Año (2 dígitos), Mes (2 dígitos), Día (2 dígitos)

            // Generar los caracteres aleatorios (restan 10 - iniciales.Length - fechaFormato.Length)
            int caracteresFaltantes = 10 - iniciales.Length - fechaFormato.Length;
            string aleatorios = GenerarCaracteresAleatorios(caracteresFaltantes);

            // Combinar todas las partes
            return $"{iniciales}{fechaFormato}{aleatorios}";
        }

        private static string GenerarCaracteresAleatorios(int cantidad)
        {
            const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(caracteresPermitidos, cantidad)
                                         .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
