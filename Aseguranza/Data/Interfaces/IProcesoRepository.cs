using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface IProcesoRepository
    {
        DataTable Consultar(string textoBuscar);

        Mensaje Guardar(Proceso proceso);

        Mensaje Borrar(int id);
    }
}