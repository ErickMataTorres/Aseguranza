using System;
using System.IO;
using System.Linq;

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

            string limpio = new string(
                texto
                    .Trim()
                    .Where(c => !caracteresInvalidos.Contains(c))
                    .ToArray()
            );

            return string.IsNullOrWhiteSpace(limpio) ? "SIN_NUMERO_RELOJ" : limpio;
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