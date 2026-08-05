using Aseguranza.Clases.Aseguranza.Clases;
using System;
using System.IO;
using System.Text;

namespace Aseguranza.Clases
{
    public static class ConfiguracionSistema
    {
        private static readonly string carpetaConfiguracion =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData),
                "Aseguranza");

        private static readonly string rutaArchivoConfiguracion =
            Path.Combine(
                carpetaConfiguracion,
                "configuracion.txt");

        private const string RutaArchivosDefault = @"C:\Aseguranza";

        private const string CadenaConexionDefault =
            @"Server=A;Database=AseguranzaBD;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        private static readonly string RutaSqliteDefault =
            Path.Combine(
                carpetaConfiguracion,
                "Data",
                "Aseguranza.db");

        public static ProveedorBaseDatos ObtenerProveedorBaseDatos()
        {
            string valor = LeerValor(
                "PROVEEDOR_BD",
                ProveedorBaseDatos.SqlServer.ToString());

            bool proveedorValido = Enum.TryParse(
                valor,
                ignoreCase: true,
                out ProveedorBaseDatos proveedor);

            if (!proveedorValido)
            {
                return ProveedorBaseDatos.SqlServer;
            }

            return proveedor;
        }

        public static string ObtenerRutaArchivos()
        {
            return LeerValor(
                "RUTA_ARCHIVOS",
                RutaArchivosDefault);
        }

        public static string ObtenerCadenaConexion()
        {
            return LeerValor(
                "CADENA_CONEXION",
                CadenaConexionDefault);
        }

        public static string ObtenerRutaSqlite()
        {
            return LeerValor(
                "RUTA_SQLITE",
                RutaSqliteDefault);
        }

        private static string LeerValor(
            string clave,
            string valorDefault)
        {
            CrearArchivoConfiguracionSiNoExiste();

            string[] lineas = File.ReadAllLines(
                rutaArchivoConfiguracion,
                Encoding.UTF8);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                if (linea.TrimStart().StartsWith("#"))
                {
                    continue;
                }

                int posicionIgual = linea.IndexOf('=');

                if (posicionIgual <= 0)
                {
                    continue;
                }

                string claveActual = linea
                    .Substring(0, posicionIgual)
                    .Trim();

                string valorActual = linea
                    .Substring(posicionIgual + 1)
                    .Trim();

                if (claveActual.Equals(
                    clave,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return valorActual;
                }
            }

            return valorDefault;
        }

        private static void CrearArchivoConfiguracionSiNoExiste()
        {
            Directory.CreateDirectory(carpetaConfiguracion);

            if (File.Exists(rutaArchivoConfiguracion))
            {
                return;
            }

            string contenido = string.Join(
                Environment.NewLine,
                new[]
                {
                    "# Configuración del sistema Aseguranza",
                    "# Permite cambiar rutas y proveedor sin recompilar.",
                    "",
                    "# Proveedor de base de datos: SqlServer o SQLite.",
                    "PROVEEDOR_BD=SqlServer",
                    "",
                    "# Conexión para SQL Server local o Azure SQL.",
                    $"CADENA_CONEXION={CadenaConexionDefault}",
                    "",
                    "# Archivo utilizado cuando PROVEEDOR_BD=SQLite.",
                    $"RUTA_SQLITE={RutaSqliteDefault}",
                    "",
                    "# Carpeta de fotografías, expedientes y documentos.",
                    $"RUTA_ARCHIVOS={RutaArchivosDefault}",
                    ""
                });

            File.WriteAllText(
                rutaArchivoConfiguracion,
                contenido,
                Encoding.UTF8);
        }
    }
}