using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface IExpedienteTrabajadorRepository
    {
        DataTable Consultar(int idTrabajador);

        Mensaje Guardar(
            ExpedienteTrabajador expediente);

        Mensaje Reemplazar(
            ExpedienteTrabajador expediente);

        Mensaje Eliminar(int id);
    }
}