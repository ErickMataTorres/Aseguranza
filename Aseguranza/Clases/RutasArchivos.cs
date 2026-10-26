using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Aseguranza.Clases
{
    public static class RutasArchivos
    {
        public static string CarpetaRaiz => ConfiguracionSistema.ObtenerRutaArchivos();

        public static string ObtenerCarpetaTrabajador(string noReloj)
        {
            string noRelojSeguro = LimpiarNombreArchivo(noReloj);
            return Path.Combine(CarpetaRaiz, "Trabajadores", noRelojSeguro);
        }

        public static string ObtenerCarpetaFotoPerfil(string noReloj)
        {
            return Path.Combine(ObtenerCarpetaTrabajador(noReloj), "FotoPerfil");
        }

        public static string ObtenerRutaFotoPerfil(string noReloj)
        {
            return Path.Combine(ObtenerCarpetaFotoPerfil(noReloj), "perfil.jpg");
        }

        // Las referencias gestionadas se guardan relativas a RUTA_ARCHIVOS,
        // tanto en instalaciones portables como en SQL Server.
        public static string ObtenerReferenciaGestionada(string rutaFisica)
        {
            string relativa = Path.GetRelativePath(CarpetaRaiz, rutaFisica);
            if (!EsRutaRelativaSegura(relativa))
                throw new ArgumentException("El archivo no pertenece a la carpeta gestionada.", nameof(rutaFisica));

            return relativa;
        }

        public static string? ResolverFotoPerfil(string? referencia, string? noReloj)
            => ResolverArchivoGestionado(referencia, noReloj, expediente: false);

        public static string? ResolverArchivoExpediente(string? referencia, string? noReloj)
            => ResolverArchivoGestionado(referencia, noReloj, expediente: true);

        private static string? ResolverArchivoGestionado(
            string? referencia, string? noReloj, bool expediente)
        {
            if (string.IsNullOrWhiteSpace(referencia))
                return null;

            try
            {
                string valor = referencia.Trim();
                if (Path.IsPathFullyQualified(valor))
                {
                    if (File.Exists(valor))
                        return valor;

                    // La recuperación se limita al mismo archivo y trabajador.
                    // No se buscan nombres de archivo en otras carpetas.
                    string[] partes = valor.Replace('/', '\\')
                        .Split('\\', StringSplitOptions.RemoveEmptyEntries);
                    if (string.IsNullOrWhiteSpace(noReloj) || partes.Length == 0 ||
                        !EsNombreArchivoSeguro(partes[^1]))
                        return null;

                    string? recuperada = null;
                    if (!expediente && partes.Length >= 4 &&
                        partes[^4].Equals("Trabajadores", StringComparison.OrdinalIgnoreCase) &&
                        partes[^3].Equals(LimpiarNombreArchivo(noReloj), StringComparison.OrdinalIgnoreCase) &&
                        partes[^2].Equals("FotoPerfil", StringComparison.OrdinalIgnoreCase))
                    {
                        recuperada = Path.Combine(ObtenerCarpetaFotoPerfil(noReloj), partes[^1]);
                    }
                    else if (expediente && partes.Length >= 5 &&
                        partes[^5].Equals("Trabajadores", StringComparison.OrdinalIgnoreCase) &&
                        partes[^4].Equals(LimpiarNombreArchivo(noReloj), StringComparison.OrdinalIgnoreCase) &&
                        partes[^3].Equals("Expediente", StringComparison.OrdinalIgnoreCase) &&
                        EsSubcarpetaExpediente(partes[^2]))
                    {
                        recuperada = Path.Combine(ObtenerCarpetaExpediente(noReloj), partes[^2], partes[^1]);
                    }

                    return recuperada is not null && File.Exists(recuperada)
                        ? recuperada : null;
                }

                string relativa = valor.Replace('\\', Path.DirectorySeparatorChar)
                    .Replace('/', Path.DirectorySeparatorChar);
                if (!EsRutaRelativaSegura(relativa) ||
                    string.IsNullOrWhiteSpace(noReloj))
                    return null;

                string ruta = Path.GetFullPath(Path.Combine(CarpetaRaiz, relativa));
                bool gestionada = expediente
                    ? EsArchivoExpedienteGestionado(ruta, noReloj)
                    : EsFotoPerfilGestionada(ruta, noReloj);
                return gestionada && File.Exists(ruta)
                    ? ruta : null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }

        public static bool EsFotoPerfilGestionada(string rutaFisica, string noReloj)
        {
            try
            {
                string relativa = Path.GetRelativePath(ObtenerCarpetaFotoPerfil(noReloj), rutaFisica);
                return EsRutaRelativaSegura(relativa) && EsNombreArchivoSeguro(relativa);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool EsArchivoExpedienteGestionado(string rutaFisica, string noReloj)
        {
            try
            {
                string relativa = Path.GetRelativePath(ObtenerCarpetaExpediente(noReloj), rutaFisica);
                if (!EsRutaRelativaSegura(relativa))
                    return false;

                string[] partes = relativa.Split(Path.DirectorySeparatorChar);
                return partes.Length == 2 && EsSubcarpetaExpediente(partes[0]) &&
                    EsNombreArchivoSeguro(partes[1]);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static bool EsNombreArchivoSeguro(string nombre)
            => !string.IsNullOrWhiteSpace(nombre) &&
               nombre is not ("." or "..") &&
               !Path.IsPathRooted(nombre) &&
               Path.GetFileName(nombre) == nombre &&
               nombre.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;

        private static bool EsSubcarpetaExpediente(string carpeta)
            => carpeta.Equals("Fotos", StringComparison.OrdinalIgnoreCase) ||
               carpeta.Equals("PDFs", StringComparison.OrdinalIgnoreCase) ||
               carpeta.Equals("Documentos", StringComparison.OrdinalIgnoreCase);

        private static bool EsRutaRelativaSegura(string ruta)
        {
            return !string.IsNullOrWhiteSpace(ruta) &&
                !Path.IsPathRooted(ruta) &&
                ruta != "." &&
                ruta != ".." &&
                !ruta.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal) &&
                !ruta.StartsWith(".." + Path.AltDirectorySeparatorChar, StringComparison.Ordinal);
        }

        public static string ObtenerCarpetaExpediente(string noReloj)
        {
            return Path.Combine(ObtenerCarpetaTrabajador(noReloj), "Expediente");
        }

        public static string ObtenerCarpetaExpedienteFotos(string noReloj)
        {
            return Path.Combine(ObtenerCarpetaExpediente(noReloj), "Fotos");
        }

        public static string ObtenerCarpetaExpedientePDFs(string noReloj)
        {
            return Path.Combine(ObtenerCarpetaExpediente(noReloj), "PDFs");
        }

        public static string ObtenerCarpetaExpedienteDocumentos(string noReloj)
        {
            return Path.Combine(ObtenerCarpetaExpediente(noReloj), "Documentos");
        }

        public static void CrearCarpetasTrabajador(string noReloj)
        {
            Directory.CreateDirectory(ObtenerCarpetaTrabajador(noReloj));
            Directory.CreateDirectory(ObtenerCarpetaFotoPerfil(noReloj));
            Directory.CreateDirectory(ObtenerCarpetaExpediente(noReloj));
            Directory.CreateDirectory(ObtenerCarpetaExpedienteFotos(noReloj));
            Directory.CreateDirectory(ObtenerCarpetaExpedientePDFs(noReloj));
            Directory.CreateDirectory(ObtenerCarpetaExpedienteDocumentos(noReloj));
        }

        private static string LimpiarNombreArchivo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "SIN_NUMERO_RELOJ";

            char[] caracteresInvalidos = Path.GetInvalidFileNameChars();

            string valor = texto.Trim();
            StringBuilder seguro = new StringBuilder();
            for (int i = 0; i < valor.Length; i++)
            {
                char caracter = valor[i];
                if (caracter == '%' || caracteresInvalidos.Contains(caracter) ||
                    (caracter == '.' && (valor is "." or ".." || i == valor.Length - 1)))
                    seguro.Append('%').Append(((int)caracter).ToString("X4"));
                else
                    seguro.Append(caracter);
            }

            return seguro.Length == 0 ? "SIN_NUMERO_RELOJ" : seguro.ToString();
        }

        public static string ObtenerCarpetaTrabajadoresEliminados()
        {
            return Path.Combine(CarpetaRaiz, "Trabajadores_Eliminados");
        }

        public static string MoverCarpetaTrabajadorAEliminados(string noReloj)
        {
            string carpetaOrigen = ObtenerCarpetaTrabajador(noReloj);

            if (!Directory.Exists(carpetaOrigen))
                return "";

            string carpetaEliminados = ObtenerCarpetaTrabajadoresEliminados();

            if (!Directory.Exists(carpetaEliminados))
                Directory.CreateDirectory(carpetaEliminados);

            string fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string carpetaDestino = Path.Combine(carpetaEliminados, $"{noReloj}_{fecha}");

            Directory.Move(carpetaOrigen, carpetaDestino);

            return carpetaDestino;
        }

    }
}
