using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ICertificacionRepository
    {
        DataTable ConsultarVerificacionNoReloj(
            string noReloj);

        DataTable ConsultarPorTrabajador(
            int idTrabajador,
            string textoBuscar);

        ExpirationDashboardResult ConsultarVencimientos(
            ExpirationDashboardRequest request);

        ExpirationReportResult ObtenerVencimientosParaReporte(
            ExpirationReportRequest request);

        Mensaje Guardar(
            Certificacion certificacion);

        Mensaje Borrar(
            int id);
    }
}
