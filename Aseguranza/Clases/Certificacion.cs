using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Certificacion
    {
        public int Id { get; set; }

        public int IdTrabajador { get; set; }

        public int IdProceso { get; set; }

        public DateTime FechaCertificacion { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public int IdCertificador { get; set; }

        public string? Comentario { get; set; }

        public static DataTable
            ConsultarVerificacionNoReloj(
                string noReloj)
        {
            return RepositorioFactory
                .CrearCertificacionRepository()
                .ConsultarVerificacionNoReloj(
                    noReloj ?? string.Empty);
        }

        public static DataTable
            ConsultarCertificacionesPorTrabajador(
                int idTrabajador,
                string textoBuscar)
        {
            return RepositorioFactory
                .CrearCertificacionRepository()
                .ConsultarPorTrabajador(
                    idTrabajador,
                    textoBuscar ?? string.Empty);
        }

        public Mensaje GuardarCertificacion()
        {
            return RepositorioFactory
                .CrearCertificacionRepository()
                .Guardar(this);
        }

        public static Mensaje BorrarCertificacion(
            int id)
        {
            return RepositorioFactory
                .CrearCertificacionRepository()
                .Borrar(id);
        }
    }
}