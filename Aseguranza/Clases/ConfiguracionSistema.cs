using System;
using System.IO;
using System.Text;

namespace Aseguranza.Clases
{
    public static class ConfiguracionSistema
    {
        private const string NombreArchivoConfiguracion =
            "configuracion.txt";

        private const string NombreIndicadorPortable =
            "portable.flag";

        private const string CadenaConexionDefault =
            @"Server=A;Database=AseguranzaBD;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        // =========================================================
        // MODO DE EJECUCIÓN
        // =========================================================

        public static bool EsModoPortable()
        {
            string rutaIndicador =
                Path.Combine(
                    AppContext.BaseDirectory,
                    NombreIndicadorPortable);

            return File.Exists(
                rutaIndicador);
        }

        public static string ObtenerCarpetaConfiguracion()
        {
            if (EsModoPortable())
            {
                return Path.GetFullPath(
                    AppContext.BaseDirectory);
            }

            return Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.CommonApplicationData),
                "Aseguranza");
        }

        public static string ObtenerRutaArchivoConfiguracion()
        {
            CrearArchivoConfiguracionSiNoExiste();

            return ObtenerRutaArchivoConfiguracionInterna();
        }

        public static bool ExisteArchivoConfiguracion()
        {
            return File.Exists(
                ObtenerRutaArchivoConfiguracionInterna());
        }

        public static string ObtenerRutaLogs()
        {
            if (EsModoPortable())
            {
                return Path.Combine(
                    AppContext.BaseDirectory,
                    "Logs");
            }

            return Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "Aseguranza",
                "Logs");
        }

        // =========================================================
        // ESTRUCTURA PORTABLE
        // =========================================================

        /// <summary>
        /// Crea la estructura mínima de carpetas de la versión portable
        /// sin leer ni crear configuracion.txt.
        /// Esto permite preparar la carpeta antes de la configuración inicial.
        /// </summary>
        public static void AsegurarEstructuraPortableBasica()
        {
            if (!EsModoPortable())
            {
                return;
            }

            string carpetaAplicacion =
                Path.GetFullPath(
                    AppContext.BaseDirectory);

            string carpetaData =
                Path.Combine(
                    carpetaAplicacion,
                    "Data");

            string carpetaArchivos =
                Path.Combine(
                    carpetaAplicacion,
                    "Archivos");

            string carpetaFotos =
                Path.Combine(
                    carpetaArchivos,
                    "Fotos");

            string carpetaExpedientes =
                Path.Combine(
                    carpetaArchivos,
                    "Expedientes");

            string carpetaLogs =
                Path.Combine(
                    carpetaAplicacion,
                    "Logs");

            string carpetaPlantillas =
                Path.Combine(
                    carpetaAplicacion,
                    "Plantillas");

            Directory.CreateDirectory(
                carpetaData);

            Directory.CreateDirectory(
                carpetaArchivos);

            Directory.CreateDirectory(
                carpetaFotos);

            Directory.CreateDirectory(
                carpetaExpedientes);

            Directory.CreateDirectory(
                carpetaLogs);

            Directory.CreateDirectory(
                carpetaPlantillas);
        }

        /// <summary>
        /// Crea las carpetas correspondientes a las rutas actualmente
        /// configuradas. Solo actúa en modo portable.
        /// </summary>
        public static void AsegurarEstructuraPortableConfigurada()
        {
            if (!EsModoPortable())
            {
                return;
            }

            AsegurarEstructuraPortableBasica();

            string rutaArchivos =
                ObtenerRutaArchivos();

            if (!string.IsNullOrWhiteSpace(
                    rutaArchivos))
            {
                Directory.CreateDirectory(
                    rutaArchivos);

                Directory.CreateDirectory(
                    Path.Combine(
                        rutaArchivos,
                        "Fotos"));

                Directory.CreateDirectory(
                    Path.Combine(
                        rutaArchivos,
                        "Expedientes"));
            }

            string rutaSqlite =
                ObtenerRutaSqlite();

            if (!string.IsNullOrWhiteSpace(
                    rutaSqlite))
            {
                string? carpetaSqlite =
                    Path.GetDirectoryName(
                        rutaSqlite);

                if (!string.IsNullOrWhiteSpace(
                        carpetaSqlite))
                {
                    Directory.CreateDirectory(
                        carpetaSqlite);
                }
            }

            Directory.CreateDirectory(
                ObtenerRutaLogs());
        }

        // =========================================================
        // CONSULTA DE CONFIGURACIÓN
        // =========================================================

        public static ProveedorBaseDatos ObtenerProveedorBaseDatos()
        {
            string valor =
                LeerValor(
                    "PROVEEDOR_BD",
                    ProveedorBaseDatos.SqlServer
                        .ToString());

            bool proveedorValido =
                Enum.TryParse(
                    valor,
                    ignoreCase: true,
                    out ProveedorBaseDatos proveedor);

            if (!proveedorValido)
            {
                return ProveedorBaseDatos.SqlServer;
            }

            return proveedor;
        }

        /// <summary>
        /// Devuelve la ruta de archivos ya resuelta como ruta absoluta.
        /// Es la que debe utilizar la aplicación al leer/escribir archivos.
        /// </summary>
        public static string ObtenerRutaArchivos()
        {
            return ResolverRutaSistema(
                ObtenerRutaArchivosConfigurada());
        }

        /// <summary>
        /// Devuelve exactamente el valor almacenado en configuracion.txt.
        /// En modo portable puede ser una ruta relativa, por ejemplo "Archivos".
        /// </summary>
        public static string ObtenerRutaArchivosConfigurada()
        {
            return LeerValor(
                "RUTA_ARCHIVOS",
                ObtenerRutaArchivosDefault());
        }

        public static string ObtenerCadenaConexion()
        {
            return LeerValor(
                "CADENA_CONEXION",
                CadenaConexionDefault);
        }

        /// <summary>
        /// Devuelve la ruta SQLite ya resuelta como ruta absoluta.
        /// </summary>
        public static string ObtenerRutaSqlite()
        {
            return ResolverRutaSistema(
                ObtenerRutaSqliteConfigurada());
        }

        /// <summary>
        /// Devuelve exactamente el valor almacenado en configuracion.txt.
        /// En modo portable puede ser "Data\Aseguranza.db".
        /// </summary>
        public static string ObtenerRutaSqliteConfigurada()
        {
            return LeerValor(
                "RUTA_SQLITE",
                ObtenerRutaSqliteDefault());
        }

        // =========================================================
        // RUTAS PORTABLES
        // =========================================================

        public static string ResolverRutaSistema(
            string ruta)
        {
            if (string.IsNullOrWhiteSpace(
                    ruta))
            {
                return string.Empty;
            }

            string rutaExpandida =
                Environment.ExpandEnvironmentVariables(
                    ruta.Trim());

            if (Path.IsPathRooted(
                    rutaExpandida))
            {
                return Path.GetFullPath(
                    rutaExpandida);
            }

            string carpetaBase =
                EsModoPortable()
                    ? AppContext.BaseDirectory
                    : ObtenerCarpetaConfiguracion();

            return Path.GetFullPath(
                Path.Combine(
                    carpetaBase,
                    rutaExpandida));
        }

        public static string ConvertirRutaParaConfiguracion(
            string ruta)
        {
            return NormalizarRutaParaGuardar(
                ruta);
        }

        // =========================================================
        // GUARDAR CONFIGURACIÓN
        // =========================================================

        public static void GuardarConfiguracion(
            ProveedorBaseDatos proveedor,
            string cadenaConexion,
            string rutaSqlite,
            string rutaArchivos)
        {
            string carpetaConfiguracion =
                ObtenerCarpetaConfiguracion();

            Directory.CreateDirectory(
                carpetaConfiguracion);

            string cadenaNormalizada =
                cadenaConexion?.Trim() ??
                string.Empty;

            string sqliteNormalizado =
                rutaSqlite?.Trim() ??
                string.Empty;

            string archivosNormalizado =
                rutaArchivos?.Trim() ??
                string.Empty;

            if (proveedor ==
                    ProveedorBaseDatos.SqlServer &&
                string.IsNullOrWhiteSpace(
                    cadenaNormalizada))
            {
                throw new InvalidOperationException(
                    "La cadena de conexión de SQL Server no puede quedar vacía.");
            }

            if (proveedor ==
                    ProveedorBaseDatos.SQLite &&
                string.IsNullOrWhiteSpace(
                    sqliteNormalizado))
            {
                throw new InvalidOperationException(
                    "La ruta de la base SQLite no puede quedar vacía.");
            }

            if (string.IsNullOrWhiteSpace(
                    archivosNormalizado))
            {
                archivosNormalizado =
                    ObtenerRutaArchivosDefault();
            }

            if (string.IsNullOrWhiteSpace(
                    sqliteNormalizado))
            {
                sqliteNormalizado =
                    ObtenerRutaSqliteDefault();
            }

            if (string.IsNullOrWhiteSpace(
                    cadenaNormalizada))
            {
                cadenaNormalizada =
                    CadenaConexionDefault;
            }

            sqliteNormalizado =
                NormalizarRutaParaGuardar(
                    sqliteNormalizado);

            archivosNormalizado =
                NormalizarRutaParaGuardar(
                    archivosNormalizado);

            string contenido =
                CrearContenidoConfiguracion(
                    proveedor,
                    cadenaNormalizada,
                    sqliteNormalizado,
                    archivosNormalizado);

            string rutaArchivoConfiguracion =
                ObtenerRutaArchivoConfiguracionInterna();

            string rutaTemporal =
                rutaArchivoConfiguracion +
                ".tmp";

            File.WriteAllText(
                rutaTemporal,
                contenido,
                Encoding.UTF8);

            File.Move(
                rutaTemporal,
                rutaArchivoConfiguracion,
                overwrite: true);

            if (EsModoPortable())
            {
                AsegurarEstructuraPortableConfigurada();
            }
        }

        // =========================================================
        // LECTURA INTERNA
        // =========================================================

        private static string LeerValor(
            string clave,
            string valorDefault)
        {
            CrearArchivoConfiguracionSiNoExiste();

            string rutaArchivoConfiguracion =
                ObtenerRutaArchivoConfiguracionInterna();

            string[] lineas =
                File.ReadAllLines(
                    rutaArchivoConfiguracion,
                    Encoding.UTF8);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(
                        linea))
                {
                    continue;
                }

                if (linea
                    .TrimStart()
                    .StartsWith("#"))
                {
                    continue;
                }

                int posicionIgual =
                    linea.IndexOf('=');

                if (posicionIgual <= 0)
                {
                    continue;
                }

                string claveActual =
                    linea
                        .Substring(
                            0,
                            posicionIgual)
                        .Trim();

                string valorActual =
                    linea
                        .Substring(
                            posicionIgual + 1)
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

        // =========================================================
        // ARCHIVO DE CONFIGURACIÓN
        // =========================================================

        private static void CrearArchivoConfiguracionSiNoExiste()
        {
            string carpetaConfiguracion =
                ObtenerCarpetaConfiguracion();

            string rutaArchivoConfiguracion =
                ObtenerRutaArchivoConfiguracionInterna();

            Directory.CreateDirectory(
                carpetaConfiguracion);

            if (File.Exists(
                    rutaArchivoConfiguracion))
            {
                return;
            }

            string contenido =
                CrearContenidoConfiguracion(
                    ProveedorBaseDatos.SqlServer,
                    CadenaConexionDefault,
                    ObtenerRutaSqliteDefault(),
                    ObtenerRutaArchivosDefault());

            File.WriteAllText(
                rutaArchivoConfiguracion,
                contenido,
                Encoding.UTF8);
        }

        private static string ObtenerRutaArchivoConfiguracionInterna()
        {
            return Path.Combine(
                ObtenerCarpetaConfiguracion(),
                NombreArchivoConfiguracion);
        }

        private static string ObtenerRutaSqliteDefault()
        {
            if (EsModoPortable())
            {
                return Path.Combine(
                    "Data",
                    "Aseguranza.db");
            }

            return Path.Combine(
                ObtenerCarpetaConfiguracion(),
                "Data",
                "Aseguranza.db");
        }

        private static string ObtenerRutaArchivosDefault()
        {
            if (EsModoPortable())
            {
                return "Archivos";
            }

            return @"C:\Aseguranza";
        }

        private static string NormalizarRutaParaGuardar(
            string ruta)
        {
            string valor =
                ruta.Trim();

            if (!EsModoPortable() ||
                string.IsNullOrWhiteSpace(
                    valor))
            {
                return valor;
            }

            string rutaAbsoluta =
                ResolverRutaSistema(
                    valor);

            string carpetaAplicacion =
                Path.GetFullPath(
                    AppContext.BaseDirectory);

            string rutaRelativa =
                Path.GetRelativePath(
                    carpetaAplicacion,
                    rutaAbsoluta);

            if (rutaRelativa.Equals(
                    "..",
                    StringComparison.Ordinal) ||
                rutaRelativa.StartsWith(
                    ".." +
                    Path.DirectorySeparatorChar,
                    StringComparison.Ordinal) ||
                rutaRelativa.StartsWith(
                    ".." +
                    Path.AltDirectorySeparatorChar,
                    StringComparison.Ordinal))
            {
                return rutaAbsoluta;
            }

            return rutaRelativa;
        }

        private static string CrearContenidoConfiguracion(
            ProveedorBaseDatos proveedor,
            string cadenaConexion,
            string rutaSqlite,
            string rutaArchivos)
        {
            return string.Join(
                Environment.NewLine,
                new[]
                {
                    "# Configuración del sistema Aseguranza",
                    "# Este archivo puede administrarse desde la ventana Configuración.",
                    $"# Modo portable: {(EsModoPortable() ? "Sí" : "No")}",
                    "",
                    "# Proveedor de base de datos: SqlServer o SQLite.",
                    $"PROVEEDOR_BD={proveedor}",
                    "",
                    "# Conexión para SQL Server local o Azure SQL.",
                    $"CADENA_CONEXION={cadenaConexion}",
                    "",
                    "# Archivo utilizado cuando PROVEEDOR_BD=SQLite.",
                    "# En modo portable puede utilizar una ruta relativa.",
                    $"RUTA_SQLITE={rutaSqlite}",
                    "",
                    "# Carpeta de fotografías, expedientes y documentos.",
                    "# En modo portable puede utilizar una ruta relativa.",
                    $"RUTA_ARCHIVOS={rutaArchivos}",
                    ""
                });
        }
    }
}
