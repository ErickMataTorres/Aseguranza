using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class ExpedienteTrabajador
    {
        public int Id { get; set; }

        public int IdTrabajador { get; set; }

        public string? NombreOriginal { get; set; }

        public string? NombreArchivo { get; set; }

        public string? Extension { get; set; }

        public string? RutaArchivo { get; set; }

        public string? TipoArchivo { get; set; }

        public string? Comentario { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public static DataTable
            ConsultarExpedienteTrabajador(
                int idTrabajador)
        {
            return RepositorioFactory
                .CrearExpedienteTrabajadorRepository()
                .Consultar(idTrabajador);
        }

        public Mensaje
            GuardarExpedienteTrabajador()
        {
            return RepositorioFactory
                .CrearExpedienteTrabajadorRepository()
                .Guardar(this);
        }

        public Mensaje
            ReemplazarExpedienteTrabajador()
        {
            return RepositorioFactory
                .CrearExpedienteTrabajadorRepository()
                .Reemplazar(this);
        }

        public static Mensaje
            EliminarExpedienteTrabajador(
                int id)
        {
            return RepositorioFactory
                .CrearExpedienteTrabajadorRepository()
                .Eliminar(id);
        }

        public static string ObtenerTipoArchivo(
            string extension)
        {
            extension =
                extension.ToLower().Trim();

            if (extension == ".jpg" ||
                extension == ".jpeg" ||
                extension == ".png" ||
                extension == ".bmp")
            {
                return "Imagen";
            }

            if (extension == ".pdf")
            {
                return "PDF";
            }

            if (extension == ".doc" ||
                extension == ".docx")
            {
                return "Word";
            }

            if (extension == ".xls" ||
                extension == ".xlsx")
            {
                return "Excel";
            }

            return "Documento";
        }
    }
}