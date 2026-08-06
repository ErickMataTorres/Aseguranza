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

        Mensaje Guardar(
            Certificacion certificacion);

        Mensaje Borrar(
            int id);
    }
}