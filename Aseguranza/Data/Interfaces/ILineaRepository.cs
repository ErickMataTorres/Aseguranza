using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ILineaRepository
    {
        DataTable Consultar(string textoBuscar);

        DataTable ConsultarPorPlanta(int idPlanta);

        Mensaje Guardar(Linea linea);

        Mensaje Borrar(int id);
    }
}