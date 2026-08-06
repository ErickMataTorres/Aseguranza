using Aseguranza.Clases;

namespace Aseguranza.Data.Interfaces
{
    public interface ICertificacionAnulacionRepository
    {
        CertificacionAnulacion? ConsultarPorCertificacion(
            int idCertificacion);

        Mensaje Guardar(
            CertificacionAnulacion anulacion);

        Mensaje Modificar(
            CertificacionAnulacion anulacion);

        Mensaje Eliminar(
            int id);
    }
}