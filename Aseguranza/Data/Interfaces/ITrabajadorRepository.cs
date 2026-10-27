using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ITrabajadorRepository
    {
        Trabajador? ConsultarPorNumeroReloj(
            string noReloj);

        DataTable Consultar(
            string textoBuscar,
            bool? activo);

        PagedTableResult ConsultarPaginado(
            WorkerPageRequest request);

        DataTable ConsultarEstadoCertificacion(
            string mostrarPor,
            string textoBuscar,
            bool? activo);

        CertificationPageResult ConsultarEstadoCertificacionPaginado(
            CertificationPageRequest request);

        DataTable ConsultarParaImportacionHdc();

        Mensaje Guardar(
            Trabajador trabajador);

        Mensaje Borrar(
            int id);

        bool CambiarEstado(
            int idTrabajador,
            bool activo,
            string origen,
            int? idImportacionHdc = null,
            string? motivo = null);
    }
}
