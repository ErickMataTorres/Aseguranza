using System;
using System.IO;
using System.Text;

namespace Aseguranza.Clases
{
    public static class ConfiguracionSistema
    {
        private static readonly string carpetaConfiguracion =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Aseguranza"
            );

        private static readonly string rutaArchivoConfiguracion =
            Path.Combine(carpetaConfiguracion, "configuracion.txt");

        private const string RutaArchivosDefault = @"C:\Aseguranza";

        private const string CadenaConexionDefault =
            @"Server=A;Database=AseguranzaBD;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        public static string ObtenerRutaArchivos()
        {
            return LeerValor("RUTA_ARCHIVOS", RutaArchivosDefault);
        }

        public static string ObtenerCadenaConexion()
        {
            return LeerValor("CADENA_CONEXION", CadenaConexionDefault);
        }

        private static string LeerValor(string clave, string valorDefault)
        {
            CrearArchivoConfiguracionSiNoExiste();

            string[] lineas = File.ReadAllLines(rutaArchivoConfiguracion, Encoding.UTF8);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                if (linea.TrimStart().StartsWith("#"))
                    continue;

                int posicionIgual = linea.IndexOf('=');

                if (posicionIgual <= 0)
                    continue;

                string claveActual = linea.Substring(0, posicionIgual).Trim();
                string valorActual = linea.Substring(posicionIgual + 1).Trim();

                if (claveActual.Equals(clave, StringComparison.OrdinalIgnoreCase))
                    return valorActual;
            }

            return valorDefault;
        }

        private static void CrearArchivoConfiguracionSiNoExiste()
        {
            if (!Directory.Exists(carpetaConfiguracion))
                Directory.CreateDirectory(carpetaConfiguracion);

            if (File.Exists(rutaArchivoConfiguracion))
                return;

            string contenido = $@"# Configuración del sistema Aseguranza
# Este archivo permite cambiar rutas y conexión sin recompilar el programa.

# Carpeta donde se guardan fotos, expedientes y documentos.
RUTA_ARCHIVOS={RutaArchivosDefault}

# Cadena de conexión a SQL Server.
CADENA_CONEXION={CadenaConexionDefault}
";

            File.WriteAllText(rutaArchivoConfiguracion, contenido, Encoding.UTF8);
        }
    }
}