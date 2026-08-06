using Aseguranza.Data;

namespace Aseguranza.Clases
{
    public class CertificacionAnulacion
    {
        public int Id { get; set; }

        public int IdCertificacion { get; set; }

        public string? TipoAnulacion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool EsPermanente { get; set; }

        public string? Comentario { get; set; }

        public bool Activa { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public static CertificacionAnulacion?
            ConsultarAnulacionPorCertificacion(
                int idCertificacion)
        {
            return RepositorioFactory
                .CrearCertificacionAnulacionRepository()
                .ConsultarPorCertificacion(
                    idCertificacion);
        }

        public Mensaje
            GuardarCertificacionAnulacion()
        {
            return RepositorioFactory
                .CrearCertificacionAnulacionRepository()
                .Guardar(this);
        }

        public Mensaje
            ModificarCertificacionAnulacion()
        {
            return RepositorioFactory
                .CrearCertificacionAnulacionRepository()
                .Modificar(this);
        }

        public static Mensaje
            EliminarCertificacionAnulacion(
                int id)
        {
            return RepositorioFactory
                .CrearCertificacionAnulacionRepository()
                .Eliminar(id);
        }
    }
}